﻿using MessagePack;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class SyncedLobbySettingsDto
    {
        [Key(0)]
        public int Id;
        [Key(1)]
        public string Name;
        [Key(2)]
        public ELobbyType Type;
        [Key(3)]
        public bool IsOfficial;
        [Key(4)]
        public int? SpawnAgainAfterDeathMs;
        [Key(5)]
        public int? BombDefuseTimeMs;
        [Key(6)]
        public int? BombPlantTimeMs;
        [Key(7)]
        public int? CountdownTime;
        [Key(8)]
        public int? RoundTime;
        [Key(9)]
        public int? BombDetonateTimeMs;
        [Key(10)]
        public int? MapLimitTime;
        [Key(11)]
        public bool InLobbyWithMaps;
        [Key(12)]
        public EMapLimitType? MapLimitType;
        [Key(13)] 
        public int StartHealth;
        [Key(14)]
        public int StartArmor;

        [IgnoreMember]
        public string Json;

        [IgnoreMember]
        public bool IsFightLobby => Type == ELobbyType.Arena || Type == ELobbyType.FightLobby;

        public SyncedLobbySettingsDto(int Id, string Name, ELobbyType Type, bool IsOfficial, int? SpawnAgainAfterDeathMs, int? BombDefuseTimeMs, int? BombPlantTimeMs,
            int? CountdownTime, int? RoundTime, int? BombDetonateTimeMs, int? MapLimitTime, bool InLobbyWithMaps, EMapLimitType? MapLimitType,
            int StartHealth, int StartArmor)
        {
            this.Id = Id;
            this.Name = Name;
            this.Type = Type;
            this.IsOfficial = IsOfficial;
            this.SpawnAgainAfterDeathMs = SpawnAgainAfterDeathMs;
            this.BombDefuseTimeMs = BombDefuseTimeMs;
            this.BombPlantTimeMs = BombPlantTimeMs;
            this.CountdownTime = CountdownTime;
            this.RoundTime = RoundTime;
            this.BombDetonateTimeMs = BombDetonateTimeMs;
            this.MapLimitTime = MapLimitTime;
            this.InLobbyWithMaps = InLobbyWithMaps;
            this.MapLimitType = MapLimitType;
            this.StartHealth = StartHealth;
            this.StartArmor = StartArmor;

            this.Json = Serializer.ToClient(this);
        }
    }
}