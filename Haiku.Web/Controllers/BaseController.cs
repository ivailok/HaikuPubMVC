using Haiku.DTO;
using Haiku.DTO.Exceptions;
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
    public abstract class BaseController : Controller
    {
        public string LoggedUserNickname
        {
            get
            {
                return ((SessionDto)Session[SessionsService.SessionTokenLabelConst]).Nickname;
            }
        }

        protected virtual async Task<ActionResult> RunAndHandleExceptions<T>(Func<T, Task<ActionResult>> func, T model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await func(model).ConfigureAwait(false);
                }
                catch (NotFoundException e)
                {
                    ModelState.AddModelError("GeneralError", e.Message);
                }
                catch (DuplicateUserNicknameException e)
                {
                    ModelState.AddModelError("GeneralError", e.Message);
                }
                catch
                {
                    ModelState.AddModelError("GeneralError", "We have a problem now. Try later.");
                }
            }

            return View(model);
        }
    }
}