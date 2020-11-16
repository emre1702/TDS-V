using Newtonsoft.Json;

namespace TDS.Shared.Data.Models
{
    public class SyncedTeamDataDto
    {
        [JsonProperty("0")]
        public int Index { get; set; }
        [JsonProperty("1")]
        public string Name { get; set; }
        [JsonProperty("2")]
        public ColorDto Color { get; set; }
        [JsonProperty("3")]
        public SyncedTeamPlayerAmountDto AmountPlayers { get; set; }
        [JsonProperty("4")]
        public bool IsSpectator => Index == 0;

        public SyncedTeamDataDto(int index, string name, ColorDto color, SyncedTeamPlayerAmountDto amountPlayers)
        {
            Index = index;
            Name = name;
            Color = color;
            AmountPlayers = amountPlayers;
        }
    }
}
