using MessagePack;
using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Server.Dto
{
    [MessagePackObject]
    #nullable disable
    public class CustomLobbyData
    {
        [Key(0)]
        public int? LobbyId { get; set; }
        [Key(1)]
        public string Name { get; set; } = "";
        [Key(2)]
        public string OwnerName { get; set; }
        [Key(3)]
        public string Password { get; set; } = "";
        [Key(4)]
        public bool ShowRanking { get; set; }
        [Key(5)]
        public short StartHealth { get; set; }
        [Key(6)]
        public short StartArmor { get; set; }
        [Key(7)]
        public short AmountLifes { get; set; }
        [Key(8)]
        public bool MixTeamsAfterRound { get; set; }
        [Key(9)]
        public int BombDetonateTimeMs { get; set; }
        [Key(10)]
        public int BombDefuseTimeMs { get; set; }
        [Key(11)]
        public int BombPlantTimeMs { get; set; }
        [Key(12)]
        public int RoundTime { get; set; }
        [Key(13)]
        public int CountdownTime { get; set; }
        [Key(14)]
        public int SpawnAgainAfterDeathMs { get; set; }
        [Key(15)]
        public int MapLimitTime { get; set; }
        [Key(16)]
        public EMapLimitType MapLimitType { get; set; }
        [Key(17)]
        public List<CustomLobbyTeamData> Teams { get; set; }
        [Key(18)]
        public List<int> Maps { get; set; }
    }
    #nullable restore
}
