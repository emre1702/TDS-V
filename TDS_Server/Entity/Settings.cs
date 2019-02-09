using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Settings
    {
        public uint Id { get; set; }
        public string GamemodeName { get; set; }
        public string MapsPath { get; set; }
        public string NewMapsPath { get; set; }
        public bool ErrorToPlayerOnNonExistentCommand { get; set; }
        public bool ToChatOnNonExistentCommand { get; set; }
        public int DistanceToSpotToPlant { get; set; }
        public int DistanceToSpotToDefuse { get; set; }
        public int SavePlayerDataCooldownMinutes { get; set; }
        public int SaveLogsCooldownMinutes { get; set; }
        public int SaveSeasonsCooldownMinutes { get; set; }
        public int TeamOrderCooldownMs { get; set; }
    }
}
