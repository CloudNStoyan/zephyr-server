using Microsoft.AspNetCore.Mvc;
using Zephyr.DAL;
using Zephyr.Infrastructure;

namespace Zephyr.Notes
{
    public class NoteController : Controller
    {
        private NoteService NoteService { get; }

        public NoteController(NoteService noteService)
        {
            this.NoteService = noteService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var note = await this.NoteService.GetNoteById(id);

            if (note == null)
            {
                return this.RedirectToAction("All");
            }

            return this.View(note);
        }

        public async Task<IActionResult> All()
        {
            var notes = await this.NoteService.GetNotes();
            return this.View(notes);
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
            var note = await this.NoteService.GetNoteById(id);

            if (note == null)
            {
                return this.RedirectToAction("All");
            }

            var model = new NoteViewModel
            {
                NoteId = note.NoteId,
                Title = note.Title,
                Content = note.Content,
                Created = note.Created
            };

            this.ViewData["actionName"] = nameof(this.Edit);
            return this.View("CreateOrEdit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NoteViewModel model)
        {
            if (!CustomValidator.Validate(model))
            {
                return this.View("CreateOrEdit", model);
            }

            var now = DateTime.Now;

            var poco = new NotePoco
            {
                Title = model.Title,
                Content = model.Content,
                Created = now,
                LastUpdated = now
            };

            int noteId = await this.NoteService.CreateNote(poco);

            return this.RedirectToAction("Index", noteId);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NoteViewModel model)
        {
            if (!CustomValidator.Validate(model))
            {
                return this.View("CreateOrEdit", model);
            }

            var now = DateTime.Now;

            var poco = new NotePoco
            {
                NoteId = model.NoteId,
                Title = model.Title,
                Content = model.Content,
                Created = model.Created,
                LastUpdated = now
            };

            await this.NoteService.UpdateNote(poco);

            return this.RedirectToAction("Index", poco.NoteId);
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
