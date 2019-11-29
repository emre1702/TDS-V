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
        public int? LobbyId;
        [Key(1)]
        public string Name = "";
        [Key(2)]
        public string OwnerName;
        [Key(3)]
        public string Password = "";
        [Key(4)]
        public bool ShowRanking;
        [Key(5)]
        public short StartHealth;
        [Key(6)]
        public short StartArmor;
        [Key(7)]
        public short AmountLifes;
        [Key(8)]
        public bool MixTeamsAfterRound;
        [Key(9)]
        public int BombDetonateTimeMs;
        [Key(10)]
        public int BombDefuseTimeMs;
        [Key(11)]
        public int BombPlantTimeMs;
        [Key(12)]
        public int RoundTime;
        [Key(13)]
        public int CountdownTime;
        [Key(14)]
        public int SpawnAgainAfterDeathMs;
        [Key(15)]
        public int MapLimitTime;
        [Key(16)]
        public EMapLimitType MapLimitType;
        [Key(17)]
        public List<CustomLobbyTeamData> Teams;
    }
    #nullable restore
}
