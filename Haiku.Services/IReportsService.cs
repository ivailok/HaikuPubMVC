using Haiku.DTO.Request;
using Haiku.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Services
{
    public interface IReportsService
    {
        Task<IEnumerable<ReportGetDto>> GetReportsAsync(ReportsGetQueryParams queryParams);
    }
}
