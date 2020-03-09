using Newtonsoft.Json;

namespace TDS_Shared.Data.Models
{
    public class SyncedScoreboardMainmenuLobbyDataDto
    {
        [JsonProperty("0")]
        public int Id;
        [JsonProperty("1")]
        public string LobbyName;
        [JsonProperty("2")]
        public bool IsOfficial;
        [JsonProperty("3")]
        public string CreatorName;
        [JsonProperty("4")]
        public string PlayersStr;
        [JsonProperty("5")]
        public int PlayersCount;

        public SyncedScoreboardMainmenuLobbyDataDto(int Id, string LobbyName, bool IsOfficial, string CreatorName, string PlayersStr, int PlayersCount)
        {
            this.Id = Id;
            this.LobbyName = LobbyName;
            this.IsOfficial = IsOfficial;
            this.CreatorName = CreatorName;
            this.PlayersStr = PlayersStr;
            this.PlayersCount = PlayersCount;
        }
    }
}
