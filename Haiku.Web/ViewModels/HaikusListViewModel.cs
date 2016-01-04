using Haiku.DTO.Request;
using Haiku.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Haiku.Web.ViewModels
{
    public class HaikusListViewModel
    {
        public HaikusListViewModel()
        {
            SortOptions = new SelectListItem[] {
                new SelectListItem()
                {
                    Text = "Date",
                    Value = "Date"
                },
                new SelectListItem()
                {
                    Text = "Rating",
                    Value = "Rating"
                }
            };
            OrderOptions = new SelectListItem[] {
                new SelectListItem()
                {
                    Text = "Descending",
                    Value = "Descending"
                },
                new SelectListItem()
                {
                    Text = "Ascending",
                    Value = "Ascending"
                }
            };
        }

        public HaikusGetQueryParams QueryParams { get; set; }

        public List<HaikuViewModel> Haikus { get; set; }

        public IEnumerable<SelectListItem> SortOptions { get; set; }

        public IEnumerable<SelectListItem> OrderOptions { get; set; }
    }
}