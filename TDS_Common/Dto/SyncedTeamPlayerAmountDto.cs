using Newtonsoft.Json;

namespace TDS_Common.Dto
{
    public class SyncedTeamPlayerAmountDto
    {
        [JsonProperty("0")]
        public uint Amount;
        [JsonProperty("1")]
        public uint AmountAlive;
    }
}
