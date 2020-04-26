using Newtonsoft.Json;

namespace TDS_Shared.Data.Models
{
    public class SyncedScoreboardLobbyDataDto
    {
        [JsonProperty("0")]
        public string Name;
        [JsonProperty("1")]
        public int PlaytimeMinutes;
        [JsonProperty("2")]
        public int Kills;
        [JsonProperty("3")]
        public int Assists;
        [JsonProperty("4")]
        public int Deaths;
        [JsonProperty("5")]
        public int TeamIndex;

        [JsonIgnore]
        public float KillsDeathsRatio => (float)Kills / (float)Deaths;

        [JsonIgnore]
        public float KillsDeathsAssistsRatio => ((float)Kills + (float)Assists / 3) / (float)Deaths;

        public SyncedScoreboardLobbyDataDto(string name, int playtimeMinutes, int kills, int assists, int deaths, int teamIndex)
        {
            Name = name;
            PlaytimeMinutes = playtimeMinutes;
            Kills = kills;
            Assists = assists;
            Deaths = deaths;
            TeamIndex = teamIndex;
        }
    }
}
