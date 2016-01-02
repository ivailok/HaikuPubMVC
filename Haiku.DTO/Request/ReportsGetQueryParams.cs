using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haiku.DTO.Request
{
    public class ReportsGetQueryParams : PagingQueryParams
    {
        public OrderType Order { get; set; }
    }
}