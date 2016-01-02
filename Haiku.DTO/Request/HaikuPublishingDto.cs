using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Haiku.DTO.Request
{
    [DataContract]
    public class HaikuPublishingDto
    {
        [Required]
        [MinLength(10)]
        [DataMember(Name = "text", IsRequired = true)]
        public string Text { get; set; }
    }
}