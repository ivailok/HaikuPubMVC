using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Haiku.Web.ViewModels
{
    public class PublishViewModel
    {
        [Required]
        [MinLength(10, ErrorMessage = "Minimum 10 symbols.")]
        public string Text { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Minimum 4 symbols.")]
        public string Password { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 128)]
        public string PublishCode { get; set; }
    }
}