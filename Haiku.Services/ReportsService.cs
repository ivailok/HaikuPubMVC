using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haiku.DTO.Request;
using Haiku.Data.Entities;
using Haiku.Data;
using Haiku.DTO.Response;

namespace Haiku.Services
{
    public class ReportsService : IReportsService
    {
        private IUnitOfWork unitOfWork;

        public ReportsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReportGetDto>> GetReportsAsync(ReportsGetQueryParams queryParams)
        {
            var preQuery = this.unitOfWork.ReportsRepository.Query();

            IOrderedQueryable<Report> sortQuery;
            if (queryParams.Order == OrderType.Ascending)
            {
                sortQuery = preQuery.OrderBy(h => h.DateSent);
            }
            else
            {
                sortQuery = preQuery.OrderByDescending(h => h.DateSent);
            }

            var pagingQuery = sortQuery.Skip(queryParams.Skip).Take(queryParams.Take);

            IList<Report> reports = await this.unitOfWork.ReportsRepository.GetAllAsync(pagingQuery);
            return reports.Select(r => Mapper.MapReportToReportGetDto(r));
        }
    }
}
