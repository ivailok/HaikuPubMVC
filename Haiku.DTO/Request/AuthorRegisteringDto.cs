﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Haiku.DTO.Request
{
    [DataContract]
    public class AuthorRegisteringDto
    {
        [DataMember(Name = "nickname", IsRequired = true)]
        public string Nickname { get; set; }
        
        [DataMember(Name = "publishCode", IsRequired = true)]
        public string PublishCode { get; set; }
    }
}