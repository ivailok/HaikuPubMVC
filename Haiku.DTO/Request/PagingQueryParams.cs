using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haiku.DTO.Request
{
    public class PagingQueryParams
    {
        public int Skip { get; set; }

        public int Take { get; set; }
    }
}