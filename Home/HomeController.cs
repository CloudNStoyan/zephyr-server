using Microsoft.AspNetCore.Mvc;
using Zephyr.Notes;

namespace Zephyr.Home;

public class HomeController : Controller
{
    private NoteService NoteService { get; }

    public HomeController(NoteService noteService)
    {
        this.NoteService = noteService;
    }
    public IActionResult Index()
    {
        return this.View();
    }
}