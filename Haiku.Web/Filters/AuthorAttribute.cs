using Haiku.DTO;
using Haiku.Services;
using Haiku.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Haiku.Web.Filters
{
    // checking if the request is from the author
    public class AuthorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sessionsService = DependencyResolver.Current.GetService<ISessionsService>();
            
            try
            {
                SessionDto session = (SessionDto)filterContext.HttpContext.Session[SessionsService.SessionTokenLabelConst];
                SessionDto newSession = sessionsService.CheckAndUpdateSessionAsync(session).Result;
                filterContext.HttpContext.Session[SessionsService.SessionTokenLabelConst] = newSession;
            }
            catch
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
                {
                    { "Controller", "Users" },
                    { "Action", "Login" },
                    { "returnUrl", filterContext.HttpContext.Request.RawUrl }
                });
            }
        }
    }
}