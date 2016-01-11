using Haiku.DTO.Request;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haiku.Web.ViewModels
{
    public class MyHaikusListViewModel
    {
        public PagingQueryParams QueryParams { get; set; }

        public IPagedList<HaikuListItem> Haikus { get; set; }
    }
}