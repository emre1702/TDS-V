using Newtonsoft.Json;
using TDS_Common.Enum;

namespace TDS_Common.Dto
{
    public class SyncedLobbySettingsDto
    {
        public int Id;
        public string Name;
        public ELobbyType Type;
        public bool IsOfficial;
        public int? SpawnAgainAfterDeathMs;
        public int? BombDefuseTimeMs;
        public int? BombPlantTimeMs;
        public int? CountdownTime;
        public int? RoundTime;
        public int? BombDetonateTimeMs;
        public int? MapLimitTime;
        public bool InLobbyWithMaps;
        public EMapLimitType? MapLimitType;

        [JsonIgnore]
        public string Json;

        public SyncedLobbySettingsDto(int Id, string Name, ELobbyType Type, bool IsOfficial, int? SpawnAgainAfterDeathMs, int? BombDefuseTimeMs, int? BombPlantTimeMs,
            int? CountdownTime, int? RoundTime, int? BombDetonateTimeMs, int? MapLimitTime, bool InLobbyWithMaps, EMapLimitType? MapLimitType)
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

            this.Json = JsonConvert.SerializeObject(this);
        }
    }
}