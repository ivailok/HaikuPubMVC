using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Haiku.DTO.Request
{
    [DataContract]
    public class HaikuRatingDto
    {
        [DataMember(Name = "rating")]
        public int Rating { get; set; }
    }
}