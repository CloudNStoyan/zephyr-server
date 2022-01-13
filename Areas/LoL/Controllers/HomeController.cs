using Microsoft.AspNetCore.Mvc;
using Zephyr.Infrastructure;

namespace Zephyr.Areas.LoL.Controllers
{
    [Area(CustomAreas.LoL)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
