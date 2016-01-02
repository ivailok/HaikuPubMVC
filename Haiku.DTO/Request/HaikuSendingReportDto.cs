using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.DTO.Request
{
    [DataContract]
    public class HaikuReportingDto
    {
        [DataMember(Name = "reason", IsRequired = true)]
        public string Reason { get; set; }
    }
}
