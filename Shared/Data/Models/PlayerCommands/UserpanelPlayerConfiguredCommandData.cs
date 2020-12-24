using Newtonsoft.Json;

namespace TDS.Shared.Data.Models.PlayerCommands
{
    public class UserpanelPlayerConfiguredCommandData
    {
        [JsonProperty("0")]
        public long Id { get; set; }

        [JsonProperty("1")]
        public int CommandId { get; set; }

        [JsonProperty("2")]
        public string CustomCommand { get; set; }
    }
}
