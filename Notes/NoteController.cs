using Microsoft.AspNetCore.Mvc;

namespace Zephyr.Notes
{
    public class NoteController : Controller
    {
        private NoteService NoteService { get; }

        public NoteController(NoteService noteService)
        {
            this.NoteService = noteService;
        }

        public async Task<IActionResult> Index()
        {
            var notes = await this.NoteService.GetNotes();
            return this.View(notes);
        }

        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var note = await this.NoteService.GetNoteById(id);

            return this.View(note);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.NoteService.DeleteNoteById(id);

            return this.RedirectToAction("Index");
        }
    }
}
