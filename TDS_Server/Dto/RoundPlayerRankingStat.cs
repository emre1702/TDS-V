using Newtonsoft.Json;
using TDS_Server.Instance.Player;

namespace TDS_Server.Dto
{
    class RoundPlayerRankingStat
    {
        public int Place { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public int Kills { get; set; }
        public int Assists { get; set; }
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
