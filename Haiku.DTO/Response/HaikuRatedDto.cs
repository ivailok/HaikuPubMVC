using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.DTO.Response
{
    [DataContract]
    public class HaikuRatedDto
    {
        [DataMember(Name = "haikuRating")]
        public double HaikuRating { get; set; }
    }
}
