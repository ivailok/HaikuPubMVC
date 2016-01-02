using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haiku.DTO.Request
{
    public enum HaikusSortBy
    {
        Date,
        Rating,
    }

    public class HaikusGetQueryParams : PagingQueryParams
    {
        public HaikusSortBy SortBy { get; set; }

        public OrderType Order { get; set; }
    }
}