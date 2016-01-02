using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Haiku.DTO.Response
{
    [DataContract]
    public class HaikuPublishedDto
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "datePublished")]
        public DateTime DatePublished { get; set; }
    }
}