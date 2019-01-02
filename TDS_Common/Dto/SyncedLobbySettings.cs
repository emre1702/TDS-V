using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Common.Dto
{
    public class SyncedLobbySettingsDto
    {
        public uint Id;
        public uint? SpawnAgainAfterDeathMs;
        public uint? BombDefuseTimeMs;
        public uint? BombPlantTimeMs;
        public uint? CountdownTime;
        public uint? RoundTime;
        public uint? BombDetonateTimeMs;
        public uint? DieAfterOutsideMapLimitTime;
        public bool InLobbyWithMaps;
    }
}
