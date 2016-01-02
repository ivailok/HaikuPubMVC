using Haiku.DTO.Exceptions;
using Haiku.DTO.Request;
using Haiku.Services;
using Haiku.Web.Filters;
using Haiku.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Haiku.Web.Controllers
{
    public class PublishController : Controller
    {
        private IUsersService usersService;

        public PublishController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET: Publishing
        
        public ActionResult Index()
        {
            PublishViewModel model = new PublishViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(PublishViewModel model, string nickname)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.usersService.PublishHaikuAsync(nickname, new HaikuPublishingDto()
                    {
                        Text = model.Text
                    });
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