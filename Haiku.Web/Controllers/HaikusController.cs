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
    public class HaikusController : Controller
    {
        private IHaikusService haikusService;

        public HaikusController(IHaikusService haikusService)
        {
            this.haikusService = haikusService;
        }

        public async Task<ActionResult> Index(HaikusGetQueryParams queryParams)
        {
            HaikusListViewModel model = new HaikusListViewModel();
            model.Haikus = (await this.haikusService.GetHaikusAsync(queryParams).ConfigureAwait(false)).Select(i => new HaikuViewModel()
            {
                Id = i.Id,
                Text = i.Text,
                Author = i.Author,
                Rating = i.Rating
            }).ToList();
            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var dto = await this.haikusService.GetHaikuAsync(id).ConfigureAwait(false);
            HaikuViewModel model = new HaikuViewModel()
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
            HaikuViewModel model = new HaikuViewModel()
            {
                Id = dto.Id,
                Text = dto.Text,
                Author = dto.Author,
                Rating = dto.Rating
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, HaikuViewModel model)
        {
            throw new NotImplementedException();
        }
        
        public async Task<ActionResult> Delete(int id)
        {
            await this.haikusService.DeleteHaikuAsync(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Haikus");
        }
    }
}