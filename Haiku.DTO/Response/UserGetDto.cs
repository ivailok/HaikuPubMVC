using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Haiku.DTO.Response
{
    [DataContract]
    public class UserGetDto
    {
        [DataMember(Name = "nickname", IsRequired = true)]
        public string Nickname { get; set; }

        [DataMember(Name = "rating")]
        public double? Rating { get; set; }

        [DataMember(Name = "haikus")]
        public IEnumerable<HaikuGetDto> Haikus { get; set; } 
    }
}