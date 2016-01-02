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
    public class RegisterController : Controller
    {
        private IUsersService usersService;

        public RegisterController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET: Register
        public ActionResult Index()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.usersService.RegisterAuthorAsync(new AuthorRegisteringDto()
                    {
                        Nickname = model.Nickname,
                        PublishCode = model.PublishCode
                    }).ConfigureAwait(false);

                    return RedirectToAction("Index", "Publish");
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
    }
}