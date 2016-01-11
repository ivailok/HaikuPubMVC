using Haiku.DTO.Exceptions;
using Haiku.DTO.Request;
using Haiku.Services;
using Haiku.Web.Filters;
using Haiku.Web.ViewModels;
using PagedList;
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
        private IHaikusService haikusService;

        public UsersController(IUsersService usersService, IHaikusService haikusService)
        {
            this.usersService = usersService;
            this.haikusService = haikusService;
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
            return await ValidateAndHandleExceptions(async (m) =>
            {
                var session = await this.usersService.RegisterAuthorAsync(new AuthorRegisteringDto()
                {
                    Nickname = m.Nickname,
                    Password = m.Password,
                }).ConfigureAwait(false);

                Session[SessionsService.SessionTokenLabelConst] = session;

                return RedirectToAction("Publish", "Haikus");
            }, model).ConfigureAwait(false);
        }
        
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            return await ValidateAndHandleExceptions(async (m) =>
            {
                var session = await this.usersService.LoginAsync(new AuthorLoginDto()
                {
                    Nickname = m.Nickname,
                    Password = m.Password
                }).ConfigureAwait(false);

                Session[SessionsService.SessionTokenLabelConst] = session;

                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Publish", "Haikus");
                }
                else
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Publish", "Haikus");
                    }
                }
            }, model).ConfigureAwait(false);
        }
        
        [Author]
        public async Task<ActionResult> Logout()
        {
            await this.usersService.LogoutAsync(LoggedUserNickname).ConfigureAwait(false);
            Session.Remove(SessionsService.SessionTokenLabelConst);
            return RedirectToAction("Login", "Users");
        }

        [Author]
        public async Task<ActionResult> Details(string nickname, PagingQueryParams queryParams)
        {
            if (queryParams.Take == 0)
            {
                queryParams.Take = 20;
            }

            MyHaikusListViewModel model = new MyHaikusListViewModel();
            var haikus = (await this.haikusService.GetHaikusForAsync(LoggedUserNickname, queryParams).ConfigureAwait(false)).Select(i => new HaikuListItem()
            {
                Id = i.Id,
                Text = i.Text,
                Rating = i.Rating
            }).ToList();
            var pageData = this.haikusService.GetHaikusForPagingMetadata(LoggedUserNickname);
            model.Haikus = new StaticPagedList<HaikuListItem>(haikus, queryParams.Skip / queryParams.Take + 1, queryParams.Take, pageData.TotalCount);
            model.QueryParams = new PagingQueryParams()
            {
                Skip = queryParams.Skip,
                Take = queryParams.Take
            };
            return View(model);
        }
    }
}