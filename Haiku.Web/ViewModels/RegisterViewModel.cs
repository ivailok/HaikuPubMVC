﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Haiku.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(4, ErrorMessage = "Minimum 4 symbols.")]
        [MaxLength(20, ErrorMessage = "Maximum 20 symbols.")]
        public string Nickname { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Minimum 4 symbols.")]
        public string Password { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 128)]
        public string PublishCode { get; set; }
    }
}