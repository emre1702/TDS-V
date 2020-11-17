using Newtonsoft.Json;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;

namespace TDS.Server.Data.Models
{
    public class RoundPlayerRankingStat
    {
        public RoundPlayerRankingStat(ITDSPlayer player)
        {
            Player = player;
            Name = player.DisplayName;
            Kills = player.CurrentRoundStats!.Kills;
            Assists = player.CurrentRoundStats!.Assists;
            Damage = player.CurrentRoundStats!.Damage;
        }

        [JsonProperty("4")]
        public int Assists { get; set; }

        [JsonProperty("5")]
        public int Damage { get; set; }

        [JsonProperty("3")]
        public int Kills { get; set; }

        [JsonProperty("1")]
        public string Name { get; set; }

        [JsonProperty("0")]
        public int Place { get; set; }

        [JsonIgnore]
        public ITDSPlayer Player { get; set; }

        [JsonProperty("2")]
        public int Points { get; set; }

    }
}
