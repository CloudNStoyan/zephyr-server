using Microsoft.AspNetCore.Mvc;
using Zephyr.Notes;

namespace Zephyr;

public class Home : Controller
{
    private NoteService NoteService { get; }

    public Home(NoteService noteService)
    {
        this.NoteService = noteService;
    }
    public async Task<IActionResult> Index()
    {
        var notes = await this.NoteService.GetNotes();
        return this.View(notes);
    }
}