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
    public class HaikusService : IHaikusService
    {
        private readonly IUnitOfWork unitOfWork;

        public HaikusService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        public async Task DeleteHaikuAsync(int haikuId)
        {
            await DeleteHaikuNFAsync(haikuId).ConfigureAwait(false);
            await this.unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task DeleteHaikuNFAsync(int haikuId)
        {
            this.unitOfWork.ReportsRepository.DeleteMany(r => r.HaikuId == haikuId);
            this.unitOfWork.RatingsRepository.DeleteMany(r => r.HaikuId == haikuId);
            await this.unitOfWork.HaikusRepository.DeleteAsync(haikuId).ConfigureAwait(false);
        }

        public async Task ModifyHaikuAsync(int haikuId, HaikuModifyDto dto)
        {
            var haiku = await this.unitOfWork.HaikusRepository.GetByIdAsync(haikuId).ConfigureAwait(false);
            if (haiku == null)
            {
                throw new KeyNotFoundException("Haiku not found.");
            }
            haiku.Text = dto.Text;
            this.unitOfWork.HaikusRepository.Update(haiku);
            await this.unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public PagingMetadata GetHaikusPagingMetadata()
        {
            PagingMetadata metadata = new PagingMetadata();
            metadata.TotalCount = this.unitOfWork.HaikusRepository.Query().Count();
            return metadata;
        }

        public async Task<IEnumerable<HaikuGetDto>> GetHaikusAsync(HaikusGetQueryParams queryParams)
        {
            var preQuery = this.unitOfWork.HaikusRepository.QueryInclude(h => h.User);

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

            var data = await this.unitOfWork.HaikusRepository.GetAllAsync(pagingQuery).ConfigureAwait(false);
            return data.Select(h => Mapper.MapHaikuEntityToHaikuGetDto(h));
        }

        public async Task<HaikuGetDto> GetHaikuAsync(int haikuId)
        {
            var haiku = await this.unitOfWork.HaikusRepository.GetByIdAsync(haikuId).ConfigureAwait(false);
            return Mapper.MapHaikuEntityToHaikuGetDto(haiku);
        }

        public async Task<HaikuRatedDto> RateAsync(int id, HaikuRatingDto dto)
        {
            var haiku = await this.unitOfWork.HaikusRepository.GetByIdAsync(id).ConfigureAwait(false);
            var rating = Mapper.MapHaikuRateDtoToHaikuRating(dto);
            haiku.Ratings.Add(rating);

            // updating haiku rating
            var oldHaikuRating = haiku.Rating;
            haiku.RatingsCount++;
            haiku.RatingsSum += rating.Value;
            haiku.Rating = ((double) haiku.RatingsSum) / haiku.RatingsCount;

            // updating user rating
            var user = await this.unitOfWork.UsersRepository.GetByIdAsync(haiku.UserId);
            if (oldHaikuRating == null)
            {
                user.HaikusRatingSum += haiku.Rating.Value;
                user.HaikusCount++;
            }
            else
            {
                user.HaikusRatingSum = user.HaikusRatingSum- oldHaikuRating.Value + haiku.Rating.Value;
            }
            user.Rating = user.HaikusRatingSum / user.HaikusCount;

            await this.unitOfWork.CommitAsync().ConfigureAwait(false);
            return new HaikuRatedDto()
            {
                HaikuRating = haiku.Rating.Value
            };
        }

        public async Task SendReport(int id, HaikuReportingDto dto)
        {
            var haiku = await this.unitOfWork.HaikusRepository.GetByIdAsync(id).ConfigureAwait(false);
            var report = Mapper.MapHaikuReportingDtoToReport(dto);
            haiku.Reports.Add(report);
            await this.unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}
