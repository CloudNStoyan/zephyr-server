using Npgsql;
using Zephyr.DAL;

namespace Zephyr.Areas.LoL.RunePage
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RunePageService
    {
        private Database Database { get; }
        public RunePageService(Database database)
        {
            this.Database = database;
        }

        public async Task<RunePagePoco[]> GetRunePages(int offset = 0, int limit = 20)
        {
            var runePagePocos = await this.Database.Query<RunePagePoco>(
                "SELECT * FROM rune_page ORDER BY rune_page_id DESC OFFSET @offset LIMIT @limit;",
                new NpgsqlParameter("offset", offset),
                new NpgsqlParameter("limit", limit)
            );

            return runePagePocos.ToArray();
        }

        public async Task<RunePagePoco?> GetRunePageById(int runePageId)
        {
            var runePagePoco = await this.Database.QueryOne<RunePagePoco>(
                "SELECT * FROM rune_page WHERE rune_page_id=@runePageId;",
                new NpgsqlParameter("runePageId", runePageId)
            );

            return runePagePoco;
        }

        public async Task CreateRunePage(RunePagePoco runePagePoco)
        {
            await this.Database.Insert(runePagePoco);
        }

        public async Task UpdateRunePage(RunePagePoco runePagePoco)
        {
            await this.Database.Update(runePagePoco);
        }
    }
}
