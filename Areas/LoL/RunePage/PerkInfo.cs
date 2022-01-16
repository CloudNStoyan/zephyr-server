using Newtonsoft.Json;

namespace Zephyr.Areas.LoL.RunePage;

public class PerkInfo
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("icon")]
    public string? Icon { get; set; }
}