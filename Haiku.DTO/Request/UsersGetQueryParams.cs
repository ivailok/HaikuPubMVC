using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haiku.DTO.Request
{
    public enum UsersSortBy
    {
        Nickname,
        Rating
    }

    public class UsersGetQueryParams : PagingQueryParams
    {
        public UsersSortBy SortBy { get; set; }

        public OrderType Order { get; set; }
    }
}