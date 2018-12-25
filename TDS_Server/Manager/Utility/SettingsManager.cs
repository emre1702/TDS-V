using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Common.Dto;
using TDS_Server.Entity;

namespace TDS_Server.Manager.Utility
{
    class SettingsManager
    {
        public static string GamemodeName { get => settings.GamemodeName; }
        public static string MapsPath { get => settings.MapsPath; }
        public static string NewMapsPath { get => settings.NewMapsPath; }
        public static bool ErrorToPlayerOnNonExistentCommand { get => settings.ErrorToPlayerOnNonExistentCommand; }
        public static bool ToChatOnNonExistentCommand { get => settings.ToChatOnNonExistentCommand; }
        public static SyncedSettingsDto SyncedSettings { get; private set; }

        private static Settings settings;

        public static async Task Load(TDSNewContext dbcontext)
        {
            settings = await dbcontext.Settings.AsNoTracking().SingleAsync();

            SyncedSettings = new SyncedSettingsDto()
            {
                DistanceToSpotToPlant = settings.DistanceToSpotToPlant,
                DistanceToSpotToDefuse = settings.DistanceToSpotToDefuse,
                RoundEndTime = 8 * 1000
            };
        }
    }
}
