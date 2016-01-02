using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Haiku.DTO.Response
{
    [DataContract]
    public class HaikuGetDto
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "text", IsRequired = true)]
        public string Text { get; set; }

        [DataMember(Name = "rating")]
        public double? Rating { get; set; }

        [DataMember(Name = "author")]
        public string Author { get; set; }
    }
}