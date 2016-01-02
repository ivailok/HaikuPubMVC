﻿using Haiku.Data.Entities;
using Haiku.DTO.Request;
using Haiku.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Services
{
    public interface IHaikusService
    {
        Task DeleteHaikuAsync(int haikuId);

        Task DeleteHaikuNFAsync(int haikuId);

        Task ModifyHaikuAsync(int haikuId, HaikuModifyDto dto);

        PagingMetadata GetHaikusPagingMetadata();

        Task<IEnumerable<HaikuGetDto>> GetHaikusAsync(HaikusGetQueryParams queryParams);

        Task<HaikuGetDto> GetHaikuAsync(int haikuId);

        Task<HaikuRatedDto> RateAsync(int id, HaikuRatingDto dto);

        Task SendReport(int id, HaikuReportingDto dto);
    }
}
