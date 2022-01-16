using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Zephyr.Areas.LoL.LCU;
using Zephyr.Areas.LoL.RunePage;
using Zephyr.Infrastructure;

namespace Zephyr.Areas.LoL.API
{
    [Area(CustomAreas.LoL)]
    public class ApiController : Controller
    {
        private LcuService LcuService { get; }
        private RunePageService RunePageService { get; }

        public ApiController(LcuService lcuService, RunePageService runePageService)
        {
            this.LcuService = lcuService;
            this.RunePageService = runePageService;
        }

        [SupportedOSPlatform("windows")]
        public async Task<IActionResult> GetRunePages()
        {
            var runePages = await this.LcuService.GetRunePages();

            return this.Json(runePages);
        }

        [SupportedOSPlatform("windows")]
        [HttpPost]
        public async Task<IActionResult> PutRunePage(int id)
        {
            var runePagePoco = await this.RunePageService.GetRunePageById(id);

            if (runePagePoco == null)
            {
                return this.Json(new { status_code = 404, error = "RunePage with that Id doesn't exist" });
            }

            await this.LcuService.ImportPage(runePagePoco);

            return this.Json(new { status_code = 200, success = true });
        } 
    }
}
