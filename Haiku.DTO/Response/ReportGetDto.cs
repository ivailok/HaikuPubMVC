using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haiku.DTO.Response
{
    public class ReportGetDto
    {
        public string Reason { get; set; }

        public DateTime DateSent { get; set; } 

        public int HaikuId { get; set; }
    }
}