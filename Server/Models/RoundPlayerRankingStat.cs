using Newtonsoft.Json;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Dto
{
    public class RoundPlayerRankingStat
    {
        [JsonProperty("0")]
        public int Place { get; set; }
        [JsonProperty("1")]
        public string Name { get; set; }
        [JsonProperty("2")]
        public int Points { get; set; }
        [JsonProperty("3")]
        public int Kills { get; set; }
        [JsonProperty("4")]
        public int Assists { get; set; }
        [JsonProperty("5")]
        public int Damage { get; set; }

        [JsonIgnore]
        public TDSPlayer Player { get; set; }
    
        public RoundPlayerRankingStat(TDSPlayer player)
        {
            Player = player;
            Name = player.DisplayName;
            Kills = player.CurrentRoundStats!.Kills;
            Assists = player.CurrentRoundStats!.Assists;
            Damage = player.CurrentRoundStats!.Damage;
        }
    }
}
