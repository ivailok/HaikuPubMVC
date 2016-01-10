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
    public class ReportsService : BaseService, IReportsService
    {

        public ReportsService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<ReportGetDto>> GetReportsAsync(ReportsGetQueryParams queryParams)
        {
            var preQuery = this.UnitOfWork.ReportsRepository.Query();

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

            IList<Report> reports = await this.UnitOfWork.ReportsRepository.GetAllAsync(pagingQuery);
            return reports.Select(r => Mapper.MapReportToReportGetDto(r));
        }
    }
}
