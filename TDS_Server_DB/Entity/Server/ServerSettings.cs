namespace TDS_Server_DB.Entity.Server
{
    public partial class ServerSettings
    {
        public short Id { get; set; }
        public string GamemodeName { get; set; }
        public string MapsPath { get; set; }
        public string NewMapsPath { get; set; }
        public string SavedMapsPath { get; set; }
        public bool ErrorToPlayerOnNonExistentCommand { get; set; }
        public bool ToChatOnNonExistentCommand { get; set; }
        public float DistanceToSpotToPlant { get; set; }
        public float DistanceToSpotToDefuse { get; set; }
        public int SavePlayerDataCooldownMinutes { get; set; }
        public int SaveLogsCooldownMinutes { get; set; }
        public int SaveSeasonsCooldownMinutes { get; set; }
        public int TeamOrderCooldownMs { get; set; }
        public float ArenaNewMapProbabilityPercent { get; set; }
        public int KillingSpreeMaxSecondsUntilNextKill { get; set; }
    }
}
