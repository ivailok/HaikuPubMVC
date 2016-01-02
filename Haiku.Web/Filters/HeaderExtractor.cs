using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace Haiku.Web.Filters
{
    public static class HeaderExtractor
    {
        public static bool ExtractHeader(
            NameValueCollection headers, string headerName, out string headerValue)
        {
            var token = headers[headerName];
            if (!string.IsNullOrEmpty(token))
            {
                headerValue = token;
                return true;
            }

            headerValue = string.Empty;
            return false;
        }
    }
}