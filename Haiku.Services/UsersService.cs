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

namespace Haiku.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHaikusService haikusService;

        public UsersService(IUnitOfWork unitOfWork, IHaikusService haikusService)
        {
            this.unitOfWork = unitOfWork;
            this.haikusService = haikusService;
        }

        public async Task<string> GetCurrentUser(string publishCode)
        {
            var user = await this.unitOfWork.UsersRepository
                .GetUniqueAsync(u => u.AccessToken == publishCode).ConfigureAwait(false);

            if (user == null)
            {
                throw new NotFoundException(string.Format("Author not found."));
            }

            return user.Nickname;
        }

        private async Task<User> FindUserByNicknameAsync(string nickname)
        {
            var user = await this.unitOfWork.UsersRepository
                .GetUniqueAsync(u => u.Nickname == nickname).ConfigureAwait(false);

            if (user == null)
            {
                throw new NotFoundException(string.Format("Author '{0}' not found.", nickname));
            }

            return user;
        }

        public async Task<bool> ConfirmAuthorIdentityAsync(string nickname, string publishCode)
        {
            var user = await FindUserByNicknameAsync(nickname).ConfigureAwait(false);
            if (user.AccessToken == publishCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ConfirmAdministratorIdentityAsync(string manageToken)
        {
            var user = await this.unitOfWork.UsersRepository.GetUniqueAsync(
                u => u.AccessToken == manageToken).ConfigureAwait(false);

            if (user != null && user.Role == UserRole.Admin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task RegisterAuthorAsync(AuthorRegisteringDto dto)
        {
            var existingUser = await this.unitOfWork.UsersRepository
                .GetUniqueAsync(u => u.Nickname == dto.Nickname).ConfigureAwait(false);
            if (existingUser != null)
            {
                throw new DuplicateUserNicknameException("Nickname is already taken.");
            }

            User user = Mapper.MapAuthorRegisterDtoToUser(dto);
            this.unitOfWork.UsersRepository.Add(user);
            await this.unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<HaikuPublishedDto> PublishHaikuAsync(string nickname, HaikuPublishingDto dto)
        {
            var user = await FindUserByNicknameAsync(nickname).ConfigureAwait(false);

            var haiku = Mapper.MapHaikuPublishingDtoToHaikuEntity(dto);
            haiku.User = user;

            var addedHaiku = this.unitOfWork.HaikusRepository.Add(haiku);
            await this.unitOfWork.CommitAsync().ConfigureAwait(false);

            var published = Mapper.MapHaikuEntityToHaikuPublishedDto(addedHaiku);
            return published;
        }

        public PagingMetadata GetUsersPagingMetadata()
        {
            PagingMetadata metadata = new PagingMetadata();
            metadata.TotalCount = this.unitOfWork.UsersRepository.Query().Count();
            return metadata;
        }

        public async Task<IEnumerable<UserGetDto>> GetUsersAsync(UsersGetQueryParams queryParams)
        {
            var searchQuery = string.IsNullOrEmpty(queryParams.SearchNickname) ? 
                this.unitOfWork.UsersRepository.Query() : 
                this.unitOfWork.UsersRepository.Query().Where(u => u.Nickname.Contains(queryParams.SearchNickname));

            // exclude administrators
            // show vip users first
            var preQuery = searchQuery
                .Where(u => u.Role != UserRole.Admin).OrderByDescending(u => u.Role);

            IOrderedQueryable<User> sortQuery;
            if (queryParams.SortBy == UsersSortBy.Nickname)
            {
                if (queryParams.Order == OrderType.Ascending)
                {
                    sortQuery = preQuery.ThenBy(u => u.Nickname);
                }
                else
                {
                    sortQuery = preQuery.ThenByDescending(u => u.Nickname);
                }
            }
            else
            {
                if (queryParams.Order == OrderType.Ascending)
                {
                    sortQuery = preQuery.ThenBy(u => u.Rating);
                }
                else
                {
                    sortQuery = preQuery.ThenByDescending(u => u.Rating);
                }
            }

            var pagingQuery = sortQuery.Skip(queryParams.Skip).Take(queryParams.Take);

            var data = await this.unitOfWork.UsersRepository.GetAllAsync(pagingQuery).ConfigureAwait(false);
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
                await this.haikusService.DeleteHaikuNFAsync(id).ConfigureAwait(false);
            }
            user.Rating = null;
            user.HaikusRatingSum = 0.0;
            user.HaikusCount = 0;
            await this.unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task DeleteProfileAsync(string nickname)
        {
            var user = await FindUserByNicknameAsync(nickname).ConfigureAwait(false);
            var haikuIds = user.Haikus.Select(h => h.Id).ToList();
            foreach (var id in haikuIds)
            {
                await this.haikusService.DeleteHaikuNFAsync(id).ConfigureAwait(false);
            }
            this.unitOfWork.UsersRepository.Delete(user);
            await this.unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task ChangeUserRoleAsync(string nickname, ChangeableUserRole role)
        {
            var user = await this.FindUserByNicknameAsync(nickname).ConfigureAwait(false);
            user.Role = (UserRole) role;
            await this.unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}
