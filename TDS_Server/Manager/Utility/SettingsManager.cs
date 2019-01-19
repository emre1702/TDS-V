using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Dto;
using TDS_Server.Entity;

namespace TDS_Server.Manager.Utility
{
    class SettingsManager
    {
        public static string GamemodeName => settings.GamemodeName;
        public static string MapsPath => settings.MapsPath;
        public static string NewMapsPath => settings.NewMapsPath;
        public static bool ErrorToPlayerOnNonExistentCommand => settings.ErrorToPlayerOnNonExistentCommand;
        public static bool ToChatOnNonExistentCommand => settings.ToChatOnNonExistentCommand;
        public static int SaveLogsCooldownMinutes => settings.SaveLogsCooldownMinutes;
        public static int SavePlayerDataCooldownMinutes => settings.SavePlayerDataCooldownMinutes;
        public static int SaveSeasonsCooldownMinutes => settings.SaveSeasonsCooldownMinutes;
        public static int DistanceToSpotToDefuse => settings.DistanceToSpotToDefuse;
        public static int DistanceToSpotToPlant => settings.DistanceToSpotToPlant;
        public static SyncedSettingsDto SyncedSettings { get; private set; }

        private static Settings settings;

        public static void Load(TDSNewContext dbcontext)
        {
            settings = dbcontext.Settings.AsNoTracking().Single();

            SyncedSettings = new SyncedSettingsDto()
            {
                DistanceToSpotToPlant = settings.DistanceToSpotToPlant,
                DistanceToSpotToDefuse = settings.DistanceToSpotToDefuse,
                RoundEndTime = 8 * 1000,
                MapChooseTime = 4 * 1000
            };
        }
    }
}
