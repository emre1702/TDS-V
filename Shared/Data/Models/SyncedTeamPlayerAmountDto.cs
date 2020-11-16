using Newtonsoft.Json;

namespace TDS.Shared.Data.Models
{
    public class SyncedTeamPlayerAmountDto
    {
        [JsonProperty("0")]
        public uint Amount;
        [JsonProperty("1")]
        public uint AmountAlive;
    }
}
