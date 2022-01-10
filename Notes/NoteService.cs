using Npgsql;
using Zephyr.DAL;

namespace Zephyr.Notes
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class NoteService
    {
        private Database Database { get; }

        public NoteService(Database database)
        {
            this.Database = database;
        }

        public async Task<NotePoco[]?> GetNotes(int offset = 0, int limit = 20)
        {
            var notePocos = await this.Database.Query<NotePoco>(
                "SELECT * FROM note OFFSET @offset LIMIT @limit;",
                new NpgsqlParameter("offset", offset),
                new NpgsqlParameter("limit", limit)
                );

            return notePocos.ToArray();
        }

        public async Task<int> CreateNote(NotePoco poco)
        {
            return await this.Database.Insert(poco);
        }

        public async Task<NotePoco?> GetNoteById(int noteId)
        {
            var notePoco = await this.Database.QueryOne<NotePoco>(
                "SELECT * FROM note WHERE note_id=@noteId;",
                new NpgsqlParameter("noteId", noteId)
            );

            return notePoco;
        }

        public async Task DeleteNoteById(int noteId)
        {
            var notePoco = await this.GetNoteById(noteId);

            if (notePoco == null)
            {
                return;
            }

            await this.Database.Delete(notePoco);
        }

        public async Task UpdateNote(NotePoco poco)
        {
            await this.Database.Update(poco);
        }
    }
}
