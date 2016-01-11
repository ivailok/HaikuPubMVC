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
    public class HaikusController : BaseController
    {
        private IHaikusService haikusService;
        private IUsersService usersService;

        public HaikusController(IHaikusService haikusService, IUsersService usersService)
        {
            this.haikusService = haikusService;
            this.usersService = usersService;
        }

        private async Task CheckIfMineAsync(int haikuId)
        {
            var author = await this.haikusService.GetHaikuAuthorAsync(haikuId).ConfigureAwait(false);
            if (author != LoggedUserNickname)
            {
                throw new DTO.Exceptions.UnauthorizedAccessException("Forbidden!");
            }
        }

        public async Task<ActionResult> Index(HaikusGetQueryParams queryParams)
        {
            if (queryParams.Take == 0)
            {
                queryParams.Take = 20;
            }

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
        
        [Author]
        public ActionResult Publish()
        {
            PublishViewModel model = new PublishViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Author]
        public async Task<ActionResult> Publish(PublishViewModel model)
        {
            return await ValidateAndHandleExceptions(async (m) =>
            {
                await this.usersService.PublishHaikuAsync(LoggedUserNickname, new HaikuPublishingDto()
                {
                    Text = model.Text
                }).ConfigureAwait(false);

                var routeParams = new RouteValueDictionary();
                routeParams.Add("Skip", 0);
                routeParams.Add("Take", 20);
                routeParams.Add("SortBy", "Date");
                routeParams.Add("Order", "Descending");
                return RedirectToAction("Index", "Haikus", routeParams);
            }, model).ConfigureAwait(false);
        }

        public async Task<ActionResult> Details(int id)
        {
            return await RunAndHandleExceptions(async (haikuId) =>
            {
                int myRating = -1;
                try
                {
                    myRating = await this.usersService.GetRatingForHaiku(LoggedUserNickname, id).ConfigureAwait(false);
                }
                catch
                {
                }

                var dto = await this.haikusService.GetHaikuAsync(id).ConfigureAwait(false);
                HaikuDetailsViewModel model = new HaikuDetailsViewModel()
                {
                    Id = dto.Id,
                    Text = dto.Text,
                    Author = dto.Author,
                    MyRating = myRating
                };
                return View(model);
            }, id);
        }

        [Author]
        public async Task<ActionResult> Edit(int id)
        {
            return await RunAndHandleExceptions(async (haikuId) =>
            {
                await CheckIfMineAsync(haikuId).ConfigureAwait(false);

                var dto = await this.haikusService.GetHaikuAsync(haikuId).ConfigureAwait(false);
                HaikuEditViewModel model = new HaikuEditViewModel()
                {
                    Id = dto.Id,
                    Text = dto.Text,
                };
                return View(model);
            }, id);
        }

        [HttpPost]
        [Author]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int haikuId, HaikuEditViewModel model)
        {
            return await ValidateAndHandleExceptions(async (m) =>
            {
                try
                {
                    await this.CheckIfMineAsync(haikuId).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    return View("Error", new ErrorViewModel() { ErrorMessage = e.Message });
                }

                await this.haikusService.ModifyHaikuAsync(haikuId, new HaikuModifyDto()
                {
                    Text = model.Text
                }).ConfigureAwait(false);

                var routeParams = new RouteValueDictionary();
                routeParams.Add("Skip", 0);
                routeParams.Add("Take", 20);
                routeParams.Add("SortBy", "Date");
                routeParams.Add("Order", "Descending");
                return RedirectToAction("Index", "Haikus", routeParams);
            }, model).ConfigureAwait(false);
        }
        
        [Author]
        public async Task<ActionResult> Delete(int id)
        {
            return await RunAndHandleExceptions(async (haikuId) =>
            {
                await CheckIfMineAsync(haikuId).ConfigureAwait(false);
                await this.haikusService.DeleteHaikuAsync(haikuId).ConfigureAwait(false);
                return RedirectToAction("Index", "Haikus");
            }, id).ConfigureAwait(false);
        }

        [Author]
        public async Task<ActionResult> Rate(int id, HaikuRatingDto dto)
        {
            return await RunAndHandleExceptions(async (haikuId) =>
            {
                await this.haikusService.RateAsync(id, dto).ConfigureAwait(false);
                return RedirectToAction("Details", "Haikus");
            }, id).ConfigureAwait(false);
        }
    }
}