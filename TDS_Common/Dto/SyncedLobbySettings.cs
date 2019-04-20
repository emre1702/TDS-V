using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Common.Dto
{
    public class SyncedLobbySettingsDto
    {
        public uint Id;
        public string Name;
        public uint? SpawnAgainAfterDeathMs;
        public uint? BombDefuseTimeMs;
        public uint? BombPlantTimeMs;
        public uint? CountdownTime;
        public uint? RoundTime;
        public uint? BombDetonateTimeMs;
        public uint? DieAfterOutsideMapLimitTime;
        public bool InLobbyWithMaps;

        [JsonIgnore]
        public string Json;

        public SyncedLobbySettingsDto(uint Id, string Name, uint? SpawnAgainAfterDeathMs, uint? BombDefuseTimeMs, uint? BombPlantTimeMs,
            uint? CountdownTime, uint? RoundTime, uint? BombDetonateTimeMs, uint? DieAfterOutsideMapLimitTime, bool InLobbyWithMaps)
        {
            this.Id = Id;
            this.Name = Name;
            this.SpawnAgainAfterDeathMs = SpawnAgainAfterDeathMs;
            this.BombDefuseTimeMs = BombDefuseTimeMs;
            this.BombPlantTimeMs = BombPlantTimeMs;
            this.CountdownTime = CountdownTime;
            this.RoundTime = RoundTime;
            this.BombDetonateTimeMs = BombDetonateTimeMs;
            this.DieAfterOutsideMapLimitTime = DieAfterOutsideMapLimitTime;
            this.InLobbyWithMaps = InLobbyWithMaps;

            this.Json = JsonConvert.SerializeObject(this);
        }
    }
}
