using MessagePack;
using TDS_Server.Instance.Player;

namespace TDS_Server.Dto
{
    [MessagePackObject]
    class RoundPlayerRankingStat
    {
        [Key(0)]
        public int Place { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public int Points { get; set; }
        [Key(3)]
        public int Kills { get; set; }
        [Key(4)]
        public int Assists { get; set; }
        [Key(5)]
        public int Damage { get; set; }

        [IgnoreMember]
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
