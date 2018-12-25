using RAGE.Elements;
using TDS_Client.Enum;
using TDS_Client.Instance.Language;
using TDS_Client.Interface;
using TDS_Common.Dto;
using TDS_Common.Enum;

namespace TDS_Client.Manager.Utility
{
    static class Settings
    {
        public const int ScreenFadeInTimeAfterSpawn = 2000;
        public const int ScreenFadeOutTimeAfterSpawn = 2000;

        public static ILanguage Language = new English();
        public static bool ShowBloodscreen = true;

        private static SyncedSettingsDto syncedSettings;
        private static SyncedLobbySettingsDto syncedLobbySettings;

        public static int DistanceToSpotToPlant => syncedSettings.DistanceToSpotToPlant;
        public static int DistanceToSpotToDefuse => syncedSettings.DistanceToSpotToDefuse;

        //public static uint BombDefuseTimeMs => syncedLobbySettings.BombDefuseTimeMs.Value;
        //public static uint BombPlantTimeMs => syncedLobbySettings.BombPlantTimeMs.Value;
        public static uint BombDetonateTimeMs => syncedLobbySettings.BombDetonateTimeMs.Value;
        //public static uint SpawnAgainAfterDeathMs => syncedLobbySettings.SpawnAgainAfterDeathMs.Value;
        public static uint CountdownTime => syncedLobbySettings.CountdownTime.Value;
        public static uint RoundTime => syncedLobbySettings.RoundTime.Value;
        public static int RoundEndTime => syncedSettings.RoundEndTime;
        public static uint DieAfterOutsideMapLimitTime => syncedLobbySettings.DieAfterOutsideMapLimitTime.Value;

        public static void Load()
        {
            Player.LocalPlayer.SetCanAttackFriendly(false, false);
        }

        public static void LoadSyncedSettings(SyncedSettingsDto loadedSyncedSettings)
        {
            syncedSettings = loadedSyncedSettings;
        }

        public static void LoadSyncedLobbySettings(SyncedLobbySettingsDto loadedSyncedLobbySettings)
        {
            syncedLobbySettings = loadedSyncedLobbySettings;
        }

        public static uint GetPlantOrDefuseTime(EPlantDefuseStatus status)
        {
            if (status == EPlantDefuseStatus.Defusing)
                return syncedLobbySettings.BombDefuseTimeMs.Value;
            else if (status == EPlantDefuseStatus.Planting)
                return syncedLobbySettings.BombPlantTimeMs.Value;
            return 0;
        }
    }
}
