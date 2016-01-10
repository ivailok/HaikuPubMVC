using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haiku.DTO.Request;
using Haiku.Data;
using Haiku.Data.Entities;
using System.Threading;
using Haiku.DTO.Response;
using Haiku.DTO.Exceptions;
using System.Linq.Expressions;
using Haiku.DTO;

namespace Haiku.Services
{
    public class UsersService : BaseService, IUsersService
    {
        private readonly IHaikusService HaikusService;
        private readonly ISessionsService SessionsService;

        public UsersService(IUnitOfWork unitOfWork, IHaikusService haikusService, ISessionsService sessionsService)
            : base(unitOfWork)
        {
            this.HaikusService = haikusService;
            this.SessionsService = sessionsService;
        }

        private async Task<User> FindUserByNicknameAsync(string nickname)
        {
            var user = await this.UnitOfWork.UsersRepository
                .GetUniqueAsync(u => u.Nickname == nickname).ConfigureAwait(false);

            if (user == null)
            {
                throw new NotFoundException(string.Format("Author '{0}' not found.", nickname));
            }

            return user;
        }

        private async Task<bool> CheckIfExists(Expression<Func<User, bool>> expr)
        {
            var user = await this.UnitOfWork.UsersRepository.GetUniqueAsync(expr);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<SessionDto> RegisterAuthorAsync(AuthorRegisteringDto dto)
        {
            var existingUser = await this.CheckIfExists(u => u.Nickname == dto.Nickname).ConfigureAwait(false);
            if (existingUser)
            {
                throw new DuplicateUserNicknameException("Nickname is already taken.");
            }

            string salt = await HashingService.GetSaltAsync().ConfigureAwait(false);
            string pass = await HashingService.GetHashAsync(dto.Password, salt, HashingType.Strong).ConfigureAwait(false);

            User user = new User()
            {
                Nickname = dto.Nickname,
                Password = pass,
                Salt = salt,
                HaikusRatingSum = 0.0,
                HaikusCount = 0
            };
            this.UnitOfWork.UsersRepository.Add(user);

            var session = await this.SessionsService.AddNewSessionAsync(user.Nickname, user.Salt).ConfigureAwait(false);

            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);

            return session;
        }

        public async Task<SessionDto> LoginAsync(AuthorLoginDto dto)
        {
            var user = await this.UnitOfWork.UsersRepository
                .GetUniqueAsync(u => u.Nickname == dto.Nickname).ConfigureAwait(false);
            if (user == null)
            {
                throw new NotFoundException("Wrong nickname or password.");
            }

            string pass = await HashingService.GetHashAsync(dto.Password, user.Salt, HashingType.Strong);
            if (pass != user.Password)
            {
                throw new NotFoundException("Wrong nickname or password.");
            }

            var session = await this.SessionsService.AddNewSessionAsync(user.Nickname, user.Salt).ConfigureAwait(false);

            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);

            return session;
        }

        public async Task LogoutAsync(string nickname)
        {
            var session = await this.UnitOfWork.SessionsRepository
                .GetLastAsync<DateTime>(s => s.Nickname == nickname, s => s.From).ConfigureAwait(false);
            this.UnitOfWork.SessionsRepository.Delete(session);
            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<HaikuPublishedDto> PublishHaikuAsync(string nickname, HaikuPublishingDto dto)
        {
            var user = await FindUserByNicknameAsync(nickname).ConfigureAwait(false);

            var haiku = Mapper.MapHaikuPublishingDtoToHaikuEntity(dto);
            haiku.User = user;

            var addedHaiku = this.UnitOfWork.HaikusRepository.Add(haiku);
            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);

            var published = Mapper.MapHaikuEntityToHaikuPublishedDto(addedHaiku);
            return published;
        }

        public PagingMetadata GetUsersPagingMetadata()
        {
            PagingMetadata metadata = new PagingMetadata();
            metadata.TotalCount = this.UnitOfWork.UsersRepository.Query().Count();
            return metadata;
        }

        public async Task<IEnumerable<UserGetDto>> GetUsersAsync(UsersGetQueryParams queryParams)
        {
            var preQuery = string.IsNullOrEmpty(queryParams.SearchNickname) ? 
                this.UnitOfWork.UsersRepository.Query() : 
                this.UnitOfWork.UsersRepository.Query().Where(u => u.Nickname.Contains(queryParams.SearchNickname));
            
            IOrderedQueryable<User> sortQuery;
            if (queryParams.SortBy == UsersSortBy.Nickname)
            {
                if (queryParams.Order == OrderType.Ascending)
                {
                    sortQuery = preQuery.OrderBy(u => u.Nickname);
                }
                else
                {
                    sortQuery = preQuery.OrderByDescending(u => u.Nickname);
                }
            }
            else
            {
                if (queryParams.Order == OrderType.Ascending)
                {
                    sortQuery = preQuery.OrderBy(u => u.Rating);
                }
                else
                {
                    sortQuery = preQuery.OrderByDescending(u => u.Rating);
                }
            }

            var pagingQuery = sortQuery.Skip(queryParams.Skip).Take(queryParams.Take);

            var data = await this.UnitOfWork.UsersRepository.GetAllAsync(pagingQuery).ConfigureAwait(false);
            return data.Select(u => Mapper.MapUserToUserGetDto(u));
        }

        public async Task<UserGetDto> GetUserAsync(string nickname)
        {
            var user = await FindUserByNicknameAsync(nickname).ConfigureAwait(false);
            return Mapper.MapUserToUserGetDto(user); 
        }

        public async Task DeleteHaikusAsync(string nickname)
        {
            var user = await FindUserByNicknameAsync(nickname).ConfigureAwait(false);
            var haikuIds = user.Haikus.Select(h => h.Id).ToList();
            foreach (var id in haikuIds)
            {
                await this.HaikusService.DeleteHaikuNFAsync(id).ConfigureAwait(false);
            }
            user.Rating = null;
            user.HaikusRatingSum = 0.0;
            user.HaikusCount = 0;
            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task DeleteProfileAsync(string nickname)
        {
            var user = await FindUserByNicknameAsync(nickname).ConfigureAwait(false);
            var haikuIds = user.Haikus.Select(h => h.Id).ToList();
            foreach (var id in haikuIds)
            {
                await this.HaikusService.DeleteHaikuNFAsync(id).ConfigureAwait(false);
            }
            this.UnitOfWork.UsersRepository.Delete(user);
            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}
