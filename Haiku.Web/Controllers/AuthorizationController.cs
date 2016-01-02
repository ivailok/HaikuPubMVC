using Haiku.DTO.Exceptions;
using Haiku.Services;
using Haiku.Web.Filters;
using Haiku.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Haiku.Web.Controllers
{
    public class AuthorizationController : Controller
    {
        private IUsersService usersService;

        public AuthorizationController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET: Authorization
        public ActionResult Index(string returnUrl)
        {
            AuthorizationViewModel model = new AuthorizationViewModel()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }
        
        [HttpPost]
        public ActionResult Authorize(AuthorizationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (usersService.ConfirmAuthorIdentityAsync(model.Nickname, model.PublishCode).Result)
                    {
                        if (Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("GeneralError", "Either nickname or secret token is incorrect.");
                    }
                }
                catch (NotFoundException e)
                {
                    ModelState.AddModelError("GeneralError", e.Message);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("GeneralError", "We have a problem now. Try later.");
                }
            }

            return View();
        }
    }
}