using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haiku.Web.ViewModels
{
    public class HaikuDetailsViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public int MyRating { get; set; }
    }
}