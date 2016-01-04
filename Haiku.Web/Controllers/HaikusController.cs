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
using System.Web.Routing;

namespace Haiku.Web.Controllers
{
    public class HaikusController : Controller
    {
        private IHaikusService haikusService;
        private IUsersService usersService;

        public HaikusController(IHaikusService haikusService, IUsersService usersService)
        {
            this.haikusService = haikusService;
            this.usersService = usersService;
        }

        public async Task<ActionResult> Index(HaikusGetQueryParams queryParams, string notification)
        {
            HaikusListViewModel model = new HaikusListViewModel();
            var haikus = (await this.haikusService.GetHaikusAsync(queryParams).ConfigureAwait(false)).Select(i => new HaikuListItem()
            {
                Id = i.Id,
                Text = i.Text,
                Author = i.Author,
                Rating = i.Rating
            }).ToList();
            var pageData = this.haikusService.GetHaikusPagingMetadata();
            model.Haikus = new StaticPagedList<HaikuListItem>(haikus, queryParams.Skip / queryParams.Take + 1, queryParams.Take, pageData.TotalCount);
            model.QueryParams = new HaikusGetQueryParams()
            {
                SortBy = queryParams.SortBy,
                Order = queryParams.Order,
                Skip = queryParams.Skip,
                Take = queryParams.Take
            };
            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var dto = await this.haikusService.GetHaikuAsync(id).ConfigureAwait(false);
            HaikuListItem model = new HaikuListItem()
            {
                Id = dto.Id,
                Text = dto.Text,
                Author = dto.Author,
                Rating = dto.Rating
            };
            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var dto = await this.haikusService.GetHaikuAsync(id).ConfigureAwait(false);
            HaikuEditViewModel model = new HaikuEditViewModel()
            {
                Id = dto.Id,
                Text = dto.Text,
            };
            return View(model);
        }

        [HttpPost]
        [Author(AuthorAuthorizationType.ManageMyHaiku)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int haikuId, HaikuEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.haikusService.ModifyHaikuAsync(haikuId, new HaikuModifyDto()
                    {
                        Text = model.Text
                    }).ConfigureAwait(false);
                    
                    var routeParams = new RouteValueDictionary();
                    routeParams.Add("Skip", 0);
                    routeParams.Add("Take", 20);
                    routeParams.Add("SortBy", "Date");
                    routeParams.Add("Order", "Descending");
                    routeParams.Add("Notification", "Successfully edited!");
                    return RedirectToAction("Index", "Haikus", routeParams);
                }
                catch (NotFoundException e)
                {
                    ModelState.AddModelError("GeneralError", e.Message);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("GeneralError", "We have problems right now. Please try again later.");
                }
            }

            return View(model);
        }
        
        public async Task<ActionResult> Delete(int id)
        {
            await this.haikusService.DeleteHaikuAsync(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Haikus");
        }
    }
}