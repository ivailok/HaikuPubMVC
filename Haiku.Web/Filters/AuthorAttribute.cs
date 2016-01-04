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
    public enum AuthorAuthorizationType
    {
        NewHaiku,
        ManageMyHaiku,
        ManageOtherHaiku
    }

    // checking if the request is from the author
    public class AuthorAttribute : ActionFilterAttribute
    {
        private const string PublishTokenHeader = "PublishCode";

        private AuthorAuthorizationType type;

        public AuthorAttribute(AuthorAuthorizationType type)
        {
            this.type = type;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var usersService = DependencyResolver.Current.GetService<IUsersService>(); ;
            var haikusService = DependencyResolver.Current.GetService<IHaikusService>();

            bool author = false;
            string nickname = string.Empty;

            if (filterContext.ActionParameters.ContainsKey("model"))
            {
                string token = (filterContext.ActionParameters["model"] as AuthorizationViewModel).PublishCode;
                if (this.type == AuthorAuthorizationType.ManageMyHaiku)
                {
                    if (filterContext.ActionParameters.ContainsKey("haikuId"))
                    {
                        var haikuId = Convert.ToInt32(filterContext.ActionParameters["haikuId"]);
                        nickname = haikusService.GetHaikuAuthorAsync(haikuId).Result;

                        if (usersService.ConfirmAuthorIdentityAsync(nickname, token).Result)
                        {
                            author = true;
                        }
                    }
                }
                else if (this.type == AuthorAuthorizationType.NewHaiku)
                {
                    nickname = usersService.GetCurrentUser(token).Result;
                    author = true;
                }
                
            }

            if (!author)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("GeneralError", "Wrong credentials.");
            }
            else
            {
                if (!filterContext.Controller.TempData.ContainsKey("nickname"))
                {
                    filterContext.Controller.TempData.Add("nickname", nickname);
                }
            }
        }
    }
}