using Microsoft.AspNetCore.Mvc;
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

            var model = RunePageViewModel.FromRunePagePoco(runePagePoco);

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

            const int perkIdsDesiredLength = 9;

            var runePagePoco = model.ToRunePagePoco();

            if (runePagePoco.PerkIds.Length != perkIdsDesiredLength)
            {
                const string errorMessage = "Error parsing Perk Ids";
                model.ErrorMessages = model.ErrorMessages != null
                    ? new List<string>(model.ErrorMessages!) { errorMessage }.ToArray()
                    : new[] { errorMessage };

                this.ViewData["actionName"] = nameof(this.Create);
                return this.View("CreateOrEdit", model);
            }

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

            const int perkIdsDesiredLength = 9;

            var runePagePoco = model.ToRunePagePoco();

            if (runePagePoco.PerkIds.Length != perkIdsDesiredLength)
            {
                const string errorMessage = "Error parsing Perk Ids";
                model.ErrorMessages = model.ErrorMessages != null
                    ? new List<string>(model.ErrorMessages!) { errorMessage }.ToArray()
                    : new[] { errorMessage };

                this.ViewData["actionName"] = nameof(this.Edit);
                return this.View("CreateOrEdit", model);
            }

            await this.RunePageService.UpdateRunePage(runePagePoco);

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var runePagePoco = await this.RunePageService.GetRunePageById(id);

            if (runePagePoco == null)
            {
                return this.RedirectToAction("All");
            }

            return this.View(runePagePoco);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var runePagePoco = await this.RunePageService.GetRunePageById(id);

            if (runePagePoco == null)
            {
                return this.RedirectToAction("All");
            }

            await this.RunePageService.DeleteRunePage(runePagePoco);

            return this.RedirectToAction("All");
        }
    }
}
