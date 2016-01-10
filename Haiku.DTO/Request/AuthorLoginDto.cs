using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.DTO.Request
{
    [DataContract]
    public class AuthorLoginDto
    {
        [DataMember(Name = "nickname", IsRequired = true)]
        public string Nickname { get; set; }

        [DataMember(Name = "password", IsRequired = true)]
        public string Password { get; set; }
    }
}
