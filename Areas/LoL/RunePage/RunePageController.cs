using Microsoft.AspNetCore.Mvc;
using Zephyr.DAL;
using Zephyr.Infrastructure;

namespace Zephyr.Areas.LoL.RunePage
{
    [Area(CustomAreas.LoL)]
    public class RunePageController : Controller
    {
        private RunePageService RunePageService { get; }

        public RunePageController(RunePageService runePageService)
        {
            this.RunePageService = runePageService;
        }

        public async Task<IActionResult> All()
        {
            var runePages = await this.RunePageService.GetRunePages();
            return this.View(runePages);
        }

        [HttpGet]
        public IActionResult Create()
        {
            this.ViewData["actionName"] = nameof(this.Create);
            return this.View("CreateOrEdit");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var runePagePoco = await this.RunePageService.GetRunePageById(id);

            if (runePagePoco == null)
            {
                return this.RedirectToAction("All");
            }

            var model = new RunePageViewModel
            {
                Name = runePagePoco.Name,
                PerkIds = string.Join(" ", runePagePoco.PerkIds.Cast<int>()),
                PrimaryStyleId = runePagePoco.PrimaryStyleId,
                SubStyleId = runePagePoco.SubStyleId,
                RunePageId = runePagePoco.RunePageId
            };

            this.ViewData["actionName"] = nameof(this.Edit);
            return this.View("CreateOrEdit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RunePageViewModel model)
        {
            if (!CustomValidator.Validate(model))
            {
                this.ViewData["actionName"] = nameof(this.Create);
                return this.View("CreateOrEdit", model);
            }

            int[] perkIds;

            try
            {
                perkIds = model.PerkIds.Split(' ').Select(int.Parse).ToArray();
            }
            catch
            {
                perkIds = Array.Empty<int>();
            }

            const int perkIdsDesiredLength = 9;

            if (perkIds.Length != perkIdsDesiredLength)
            {
                const string errorMessage = "Error parsing Perk Ids";
                model.ErrorMessages = model.ErrorMessages != null
                    ? new List<string>(model.ErrorMessages!) { errorMessage }.ToArray()
                    : new[] { errorMessage };

                this.ViewData["actionName"] = nameof(this.Create);
                return this.View("CreateOrEdit", model);
            }

            var runePagePoco = new RunePagePoco
            {
                Name = model.Name,
                PerkIds = perkIds,
                PrimaryStyleId = model.PrimaryStyleId,
                SubStyleId = model.SubStyleId
            };

            await this.RunePageService.CreateRunePage(runePagePoco);

            return this.RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RunePageViewModel model)
        {
            if (!CustomValidator.Validate(model))
            {
                this.ViewData["actionName"] = nameof(this.Create);
                return this.View("CreateOrEdit", model);
            }

            int[] perkIds;

            try
            {
                perkIds = model.PerkIds.Split(' ').Select(int.Parse).ToArray();
            }
            catch
            {
                perkIds = Array.Empty<int>();
            }

            const int perkIdsDesiredLength = 9;

            if (perkIds.Length != perkIdsDesiredLength)
            {
                const string errorMessage = "Error parsing Perk Ids";
                model.ErrorMessages = model.ErrorMessages != null
                    ? new List<string>(model.ErrorMessages!) { errorMessage }.ToArray()
                    : new[] { errorMessage };

                this.ViewData["actionName"] = nameof(this.Edit);
                return this.View("CreateOrEdit", model);
            }

            var runePagePoco = new RunePagePoco
            {
                Name = model.Name,
                PerkIds = perkIds,
                PrimaryStyleId = model.PrimaryStyleId,
                SubStyleId = model.SubStyleId,
                RunePageId = model.RunePageId
            };

            await this.RunePageService.UpdateRunePage(runePagePoco);

            return this.RedirectToAction("All");
        }
    }
}
