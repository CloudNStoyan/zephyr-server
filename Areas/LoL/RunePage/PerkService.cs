using Newtonsoft.Json;

namespace Zephyr.Areas.LoL.RunePage
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PerkService
    {
        private IWebHostEnvironment Env { get; }

        public PerkInfo[] Perks { get; set; } = Array.Empty<PerkInfo>();

        public PerkService(IWebHostEnvironment env)
        {
            this.Env = env;

            this.LoadPerks();
        }

        private void LoadPerks()
        {
            string perksPath = Path.Combine(this.Env.WebRootPath, "js/lol/perks.json");

            if (!File.Exists(perksPath))
            {
                throw new Exception($"Can't find file at: '{perksPath}'");
            }

            string perkJson = File.ReadAllText(perksPath);

            var perks = JsonConvert.DeserializeObject<PerkInfo[]>(perkJson);

            if (perks == null)
            {
                throw new Exception($"Failed to deserialize '{perksPath}' as '{nameof(PerkInfo)}'");
            }

            const string iconPathPrefix = "/static/lol/";

            foreach (var perkInfo in perks)
            {
                perkInfo.Icon = iconPathPrefix + perkInfo.Icon;
            }

            this.Perks = perks;
        }
    }
}
