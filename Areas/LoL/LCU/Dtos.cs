using Newtonsoft.Json;

namespace Zephyr.Areas.LoL.LCU
{
    public class RunePageDto
    {
        [JsonProperty("id")] 
        public long Id { get; set; }

        [JsonProperty("name")] 
        public string? Name { get; set; }

        [JsonProperty("primaryStyleId")] 
        public int PrimaryStyleId { get; set; }

        [JsonProperty("selectedPerkIds")] 
        public int[]? SelectedPerkIds { get; set; }

        [JsonProperty("subStyleId")] 
        public int SubStyleId { get; set; }
    }
}