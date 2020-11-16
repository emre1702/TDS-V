using Newtonsoft.Json;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Data.Models.Userpanel.Stats
{
    public class PlayerUserpanelLobbyStats
    {
        public PlayerUserpanelLobbyStats(PlayerLobbyStats stats)
        {
            Lobby = stats.Lobby.Name;

            Kills = stats.Kills;
            Assists = stats.Assists;
            Deaths = stats.Deaths;
            Damage = stats.Damage;

            TotalKills = stats.TotalKills;
            TotalAssists = stats.TotalAssists;
            TotalDeaths = stats.TotalDeaths;
            TotalDamage = stats.TotalDamage;

            TotalRounds = stats.TotalRounds;
            MostKillsInARound = stats.MostKillsInARound;
            MostDamageInARound = stats.MostDamageInARound;
            MostAssistsInARound = stats.MostAssistsInARound;
        }

        [JsonProperty("2")]
        public int Assists { get; set; }

        [JsonProperty("4")]
        public int Damage { get; set; }

        [JsonProperty("3")]
        public int Deaths { get; set; }

        [JsonProperty("1")]
        public int Kills { get; set; }

        [JsonProperty("0")]
        public string Lobby { get; internal set; }

        [JsonProperty("12")]
        public int MostAssistsInARound { get; set; }

        [JsonProperty("11")]
        public int MostDamageInARound { get; set; }

        [JsonProperty("10")]
        public int MostKillsInARound { get; set; }

        [JsonProperty("6")]
        public int TotalAssists { get; set; }

        [JsonProperty("8")]
        public int TotalDamage { get; set; }

        [JsonProperty("7")]
        public int TotalDeaths { get; set; }

        [JsonProperty("5")]
        public int TotalKills { get; set; }

        [JsonProperty("13")]
        public int TotalMapsBought { get; set; }

        [JsonProperty("9")]
        public int TotalRounds { get; set; }
    }
}
