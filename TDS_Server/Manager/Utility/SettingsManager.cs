using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TDS_Common.Dto;
using TDS_Server.Dto;
using TDS_Server_DB.Entity;
using Command = TDS_Server_DB.Entity.Command.Commands;
using TDS_Server_DB.Entity.Server;
using TDS_Server.Instance.Player;

namespace TDS_Server.Manager.Utility
{
    internal class SettingsManager
    {
        public static string ConnectionString => _localSettings.ConnectionString.Value;
        public static string MapsPath => _serverSettings.MapsPath;
        public static string NewMapsPath => _serverSettings.NewMapsPath;
        public static string SavedMapsPath => _serverSettings.SavedMapsPath;
        public static bool ErrorToPlayerOnNonExistentCommand => _serverSettings.ErrorToPlayerOnNonExistentCommand;
        public static bool ToChatOnNonExistentCommand => _serverSettings.ToChatOnNonExistentCommand;
        public static int SaveLogsCooldownMinutes => _serverSettings.SaveLogsCooldownMinutes;
        public static int SavePlayerDataCooldownMinutes => _serverSettings.SavePlayerDataCooldownMinutes;
        public static int SaveSeasonsCooldownMinutes => _serverSettings.SaveSeasonsCooldownMinutes;
        public static float DistanceToSpotToDefuse => _serverSettings.DistanceToSpotToDefuse;
        public static float DistanceToSpotToPlant => _serverSettings.DistanceToSpotToPlant;
        public static float ArenaNewMapProbabilityPercent => _serverSettings.ArenaNewMapProbabilityPercent;
        public static int KillingSpreeMaxSecondsUntilNextKill => _serverSettings.KillingSpreeMaxSecondsUntilNextKill;
        public static int MapRatingAmountForCheck => _serverSettings.MapRatingAmountForCheck;
        public static float GiveMoneyFee => _serverSettings.GiveMoneyFee;
        public static int GiveMoneyMinAmount => _serverSettings.GiveMoneyMinAmount;

        #nullable disable warnings
        public static SyncedServerSettingsDto SyncedSettings { get; private set; }

        private static AppConfigDto _localSettings;
        private static ServerSettings _serverSettings;
        private static Command _loadMapOfOthersRightInfos;
        #nullable restore warnings

        public static void LoadLocal()
        {
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name ?? "TDS_Server";
            using var fileStream = new FileStream(appName + ".config", FileMode.Open);
            using var reader = XmlReader.Create(fileStream);
            var xmlSerializer = new XmlSerializer(typeof(AppConfigDto));

            _localSettings = (AppConfigDto)xmlSerializer.Deserialize(reader);
        }

        public static async Task Load(TDSNewContext dbcontext)
        {
            _serverSettings = await dbcontext.ServerSettings.SingleAsync();

            SyncedSettings = new SyncedServerSettingsDto()
            {
                DistanceToSpotToPlant = _serverSettings.DistanceToSpotToPlant,
                DistanceToSpotToDefuse = _serverSettings.DistanceToSpotToDefuse,
                RoundEndTime = 8 * 1000,
                MapChooseTime = 4 * 1000,
                TeamOrderCooldownMs = _serverSettings.TeamOrderCooldownMs,
                NametagMaxDistance = _serverSettings.NametagMaxDistance,
                ShowNametagOnlyOnAiming = _serverSettings.ShowNametagOnlyOnAiming
            };

            _loadMapOfOthersRightInfos = dbcontext.Commands.First(c => c.Command == "LoadMapOfOthers");

            NAPI.Server.SetGamemodeName(_serverSettings.GamemodeName);
        }

        public static bool CanLoadMapsFromOthers(TDSPlayer player)
        {
            return _loadMapOfOthersRightInfos.NeededAdminLevel.HasValue && _loadMapOfOthersRightInfos.NeededAdminLevel <= player.AdminLevel.Level
                || _loadMapOfOthersRightInfos.VipCanUse && player.Entity?.IsVip == true;

        }
    }
}