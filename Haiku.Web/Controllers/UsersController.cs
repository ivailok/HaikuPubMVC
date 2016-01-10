using Haiku.DTO.Exceptions;
using Haiku.DTO.Request;
using Haiku.Services;
using Haiku.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Haiku.Web.Controllers
{
    public class UsersController : BaseController
    {
        private IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET: Register
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var session = await this.usersService.RegisterAuthorAsync(new AuthorRegisteringDto()
                    {
                        Nickname = model.Nickname,
                        Password = model.Password,
                    }).ConfigureAwait(false);

                    Session[SessionsService.SessionTokenLabelConst] = session;

                    return RedirectToAction("Publish", "Haikus");
                }
                catch (DuplicateUserNicknameException e)
                {
                    ModelState.AddModelError("GeneralError", e.Message);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("GeneralError", "We have a problem now. Try later.");
                }
            }

            return View(model);
        }
        
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var session = await this.usersService.LoginAsync(new AuthorLoginDto()
                    {
                        Nickname = model.Nickname,
                        Password = model.Password
                    }).ConfigureAwait(false);

                    Session[SessionsService.SessionTokenLabelConst] = session;

                    return RedirectToAction("Publish", "Haikus");
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

            return View(model);
        }
        
        public async Task<ActionResult> Logout()
        {
            await this.usersService.LogoutAsync(LoggedUserNickname).ConfigureAwait(false);
            Session.Remove(SessionsService.SessionTokenLabelConst);
            return RedirectToAction("Login", "Users");
        }
    }
}