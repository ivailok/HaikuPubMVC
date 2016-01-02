using Haiku.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haiku.Web.ViewModels
{
    public class HaikusListViewModel
    {
        public HaikusListViewModel()
        {
            SortOptions = new string[] { "Date", "Rating" };
            OrderOptions = new string[] { "Descending", "Ascending" };
        }

        public List<HaikuViewModel> Haikus { get; set; }

        public string[] SortOptions { get; set; }

        public string[] OrderOptions { get; set; }
    }
}