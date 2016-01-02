using Haiku.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Haiku.Web.Filters
{
    public class AuthorAttribute : AuthorizeAttribute
    {
        private const string PublishTokenHeader = "PublishCode";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
            {
                { "Controller", "Authorization" },
                { "Action", "Index" },
                { "returnUrl", filterContext.HttpContext.Request.RawUrl }
            });
        }
    }
}