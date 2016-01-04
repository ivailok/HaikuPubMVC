using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Haiku.Web.ViewModels
{
    public class PublishViewModel : AuthorizationViewModel
    {
        [Required]
        [MinLength(10, ErrorMessage = "Minimum 10 symbols.")]
        public string Text { get; set; }
    }
}