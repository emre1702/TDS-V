using Newtonsoft.Json;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;

namespace TDS_Common.Dto
{
    public class SyncedLobbySettingsDto
    {
        [JsonProperty("0")]
        public int Id;
        [JsonProperty("1")]
        public string Name;
        [JsonProperty("2")]
        public ELobbyType Type;
        [JsonProperty("3")]
        public bool IsOfficial;
        [JsonProperty("4")]
        public int? SpawnAgainAfterDeathMs;
        [JsonProperty("5")]
        public int? BombDefuseTimeMs;
        [JsonProperty("6")]
        public int? BombPlantTimeMs;
        [JsonProperty("7")]
        public int? CountdownTime;
        [JsonProperty("8")]
        public int? RoundTime;
        [JsonProperty("9")]
        public int? BombDetonateTimeMs;
        [JsonProperty("10")]
        public int? MapLimitTime;
        [JsonProperty("11")]
        public bool InLobbyWithMaps;
        [JsonProperty("12")]
        public EMapLimitType? MapLimitType;
        [JsonProperty("13")] 
        public int StartHealth;
        [JsonProperty("14")]
        public int StartArmor;

        [JsonIgnore]
        public string Json;

        [JsonIgnore]
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
