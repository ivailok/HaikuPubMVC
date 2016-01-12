using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haiku.DTO.Request;
using Haiku.DTO.Response;
using Haiku.Data;
using Haiku.Data.Entities;
using System.Linq.Expressions;

namespace Haiku.Services
{
    public class HaikusService : BaseService, IHaikusService
    {
        public HaikusService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        
        public async Task<string> GetHaikuAuthorAsync(int haikuId)
        {
            var haiku = await this.UnitOfWork.HaikusRepository.GetByIdAsync(haikuId).ConfigureAwait(false);
            return haiku.User.Nickname;
        }

        public async Task DeleteHaikuAsync(int haikuId)
        {
            await DeleteHaikuNFAsync(haikuId).ConfigureAwait(false);
            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task DeleteHaikuNFAsync(int haikuId)
        {
            this.UnitOfWork.ReportsRepository.DeleteMany(r => r.HaikuId == haikuId);
            this.UnitOfWork.RatingsRepository.DeleteMany(r => r.HaikuId == haikuId);
            await this.UnitOfWork.HaikusRepository.DeleteAsync(haikuId).ConfigureAwait(false);
        }

        public async Task ModifyHaikuAsync(int haikuId, HaikuModifyDto dto)
        {
            var haiku = await this.UnitOfWork.HaikusRepository.GetByIdAsync(haikuId).ConfigureAwait(false);
            if (haiku == null)
            {
                throw new KeyNotFoundException("Haiku not found.");
            }
            haiku.Text = dto.Text;
            this.UnitOfWork.HaikusRepository.Update(haiku);
            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public PagingMetadata GetHaikusPagingMetadata()
        {
            PagingMetadata metadata = new PagingMetadata();
            metadata.TotalCount = this.UnitOfWork.HaikusRepository.Query().Count();
            return metadata;
        }

        public async Task<IEnumerable<HaikuGetDto>> GetHaikusAsync(HaikusGetQueryParams queryParams)
        {
            var preQuery = this.UnitOfWork.HaikusRepository.QueryInclude(h => h.User);

            IOrderedQueryable<HaikuEntity> sortQuery;
            if (queryParams.SortBy == HaikusSortBy.Date)
            {
                if (queryParams.Order == OrderType.Ascending)
                {
                    sortQuery = preQuery.OrderBy(h => h.DatePublished);
                }
                else
                {
                    sortQuery = preQuery.OrderByDescending(h => h.DatePublished);
                }
            }
            else
            {
                if (queryParams.Order == OrderType.Ascending)
                {
                    sortQuery = preQuery.OrderBy(h => h.Rating);
                }
                else
                {
                    sortQuery = preQuery.OrderByDescending(h => h.Rating);
                }
            }

            var pagingQuery = sortQuery.Skip(queryParams.Skip).Take(queryParams.Take);

            var data = await this.UnitOfWork.HaikusRepository.GetAllAsync(pagingQuery).ConfigureAwait(false);
            return data.Select(h => Mapper.MapHaikuEntityToHaikuGetDto(h));
        }

        public async Task<HaikuGetDto> GetHaikuAsync(int haikuId)
        {
            var haiku = await this.UnitOfWork.HaikusRepository.GetByIdAsync(haikuId).ConfigureAwait(false);
            return Mapper.MapHaikuEntityToHaikuGetDto(haiku);
        }

        private async Task RateExistingAsync(HaikuRating existingRating, HaikuRatingDto dto)
        {
            var haiku = await this.UnitOfWork.HaikusRepository.GetByIdAsync(existingRating.HaikuId).ConfigureAwait(false);

            var oldRating = existingRating.Value;
            existingRating.Value = dto.Rating;

            var oldHaikuRating = haiku.Rating;
            haiku.RatingsSum = haiku.RatingsSum - oldRating + dto.Rating;
            haiku.Rating = ((double)haiku.RatingsSum) / haiku.RatingsCount;

            var user = await this.UnitOfWork.UsersRepository.GetByIdAsync(haiku.UserId);
            user.HaikusRatingSum = user.HaikusRatingSum - oldHaikuRating.Value + haiku.Rating.Value;
            user.Rating = user.HaikusRatingSum / user.HaikusCount;

            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task RateAsync(string nickname, int haikuId, HaikuRatingDto dto)
        {
            var userId = (await this.UnitOfWork.UsersRepository
                .GetUniqueAsync(u => u.Nickname == nickname).ConfigureAwait(false)).Id;
            var existingRating = await this.UnitOfWork.RatingsRepository.GetByIdAsync(userId, haikuId).ConfigureAwait(false);

            if (existingRating != null)
            {
                await RateExistingAsync(existingRating, dto).ConfigureAwait(false);
            }
            else
            {
                var haiku = await this.UnitOfWork.HaikusRepository.GetByIdAsync(haikuId).ConfigureAwait(false);
                var rating = Mapper.MapHaikuRateDtoToHaikuRating(dto);
                haiku.Ratings.Add(rating);

                // updating haiku rating
                var oldHaikuRating = haiku.Rating;
                haiku.RatingsCount++;
                haiku.RatingsSum += rating.Value;
                haiku.Rating = ((double)haiku.RatingsSum) / haiku.RatingsCount;

                // updating user rating
                var user = await this.UnitOfWork.UsersRepository.GetByIdAsync(haiku.UserId);
                if (oldHaikuRating == null)
                {
                    user.HaikusRatingSum += haiku.Rating.Value;
                    user.HaikusCount++;
                }
                else
                {
                    user.HaikusRatingSum = user.HaikusRatingSum - oldHaikuRating.Value + haiku.Rating.Value;
                }
                user.Rating = user.HaikusRatingSum / user.HaikusCount;

                rating.UserId = userId;

                await this.UnitOfWork.CommitAsync().ConfigureAwait(false);
            }
        }

        public async Task SendReport(int id, HaikuReportingDto dto)
        {
            var haiku = await this.UnitOfWork.HaikusRepository.GetByIdAsync(id).ConfigureAwait(false);
            var report = Mapper.MapHaikuReportingDtoToReport(dto);
            haiku.Reports.Add(report);
            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public PagingMetadata GetHaikusForPagingMetadata(string nickname)
        {
            PagingMetadata metadata = new PagingMetadata();
            metadata.TotalCount = this.UnitOfWork.HaikusRepository
                .QueryInclude(h => h.User)
                .Where(h => h.User.Nickname == nickname)
                .Count();
            return metadata;
        }

        public async Task<IEnumerable<HaikuGetDto>> GetHaikusForAsync(string nickname, PagingQueryParams queryParams)
        {
            var user = await this.UnitOfWork.UsersRepository
                .GetUniqueAsync(u => u.Nickname == nickname).ConfigureAwait(false);

            var query = this.UnitOfWork.HaikusRepository
                .QueryInclude(h => h.User)
                .Where(h => h.User.Nickname == nickname)
                .OrderByDescending(h => h.DatePublished)
                .Skip(queryParams.Skip)
                .Take(queryParams.Take);

            var haikus = await this.UnitOfWork.HaikusRepository.GetAllAsync(query).ConfigureAwait(false);

            return haikus.Select(h => Mapper.MapHaikuEntityToHaikuGetDto(h));
        }
    }
}
