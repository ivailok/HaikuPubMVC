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
using System.Web.Routing;

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
        [ValidateAntiForgeryToken]
        [Author(AuthorAuthorizationType.NewHaiku)]
        public async Task<ActionResult> Publish(PublishViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.usersService.PublishHaikuAsync(TempData["nickname"].ToString(), new HaikuPublishingDto()
                    {
                        Text = model.Text
                    });

                    var routeParams = new RouteValueDictionary();
                    routeParams.Add("Skip", 0);
                    routeParams.Add("Take", 20);
                    routeParams.Add("SortBy", "Date");
                    routeParams.Add("Order", "Descending");
                    return RedirectToAction("Index", "Haikus", routeParams);
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