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

        public async Task<NotePoco[]> GetNotes(int offset = 0, int limit = 20)
        {
            var notePocos = await this.Database.Query<NotePoco>(
                "SELECT * FROM note OFFSET @offset LIMIT @limit;",
                new NpgsqlParameter("offset", offset),
                new NpgsqlParameter("limit", limit)
                );

            return notePocos.ToArray();
        }
    }
}
