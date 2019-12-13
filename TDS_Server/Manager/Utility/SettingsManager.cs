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
        public static bool ErrorToPlayerOnNonExistentCommand => ServerSettings.ErrorToPlayerOnNonExistentCommand;
        public static bool ToChatOnNonExistentCommand => ServerSettings.ToChatOnNonExistentCommand;
        public static int SaveLogsCooldownMinutes => ServerSettings.SaveLogsCooldownMinutes;
        public static int SavePlayerDataCooldownMinutes => ServerSettings.SavePlayerDataCooldownMinutes;
        public static int SaveSeasonsCooldownMinutes => ServerSettings.SaveSeasonsCooldownMinutes;
        public static float DistanceToSpotToDefuse => ServerSettings.DistanceToSpotToDefuse;
        public static float DistanceToSpotToPlant => ServerSettings.DistanceToSpotToPlant;
        public static float ArenaNewMapProbabilityPercent => ServerSettings.ArenaNewMapProbabilityPercent;
        public static int KillingSpreeMaxSecondsUntilNextKill => ServerSettings.KillingSpreeMaxSecondsUntilNextKill;
        public static int MapRatingAmountForCheck => ServerSettings.MapRatingAmountForCheck;
        public static float MinMapRatingForNewMaps => ServerSettings.MinMapRatingForNewMaps;
        public static float GiveMoneyFee => ServerSettings.GiveMoneyFee;
        public static int GiveMoneyMinAmount => ServerSettings.GiveMoneyMinAmount;
        public static float MultiplierRankingKills => ServerSettings.MultiplierRankingKills;
        public static float MultiplierRankingAssists => ServerSettings.MultiplierRankingAssists;
        public static float MultiplierRankingDamage => ServerSettings.MultiplierRankingDamage;

#nullable disable warnings
        public static SyncedServerSettingsDto SyncedSettings { get; private set; }

        private static AppConfigDto _localSettings;
        public static ServerSettings ServerSettings;
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

        public static async Task Load(TDSDbContext dbcontext)
        {
            ServerSettings = await dbcontext.ServerSettings.SingleAsync();

            SyncedSettings = new SyncedServerSettingsDto()
            {
                DistanceToSpotToPlant = ServerSettings.DistanceToSpotToPlant,
                DistanceToSpotToDefuse = ServerSettings.DistanceToSpotToDefuse,
                RoundEndTime = 8 * 1000,
                MapChooseTime = 4 * 1000,
                TeamOrderCooldownMs = ServerSettings.TeamOrderCooldownMs,
                NametagMaxDistance = ServerSettings.NametagMaxDistance,
                ShowNametagOnlyOnAiming = ServerSettings.ShowNametagOnlyOnAiming,
                AFKKickAfterSec = ServerSettings.AFKKickAfterSec
            };

            _loadMapOfOthersRightInfos = dbcontext.Commands.First(c => c.Command == "LoadMapOfOthers");

            NAPI.Server.SetGamemodeName(ServerSettings.GamemodeName);
        }

        public static bool CanLoadMapsFromOthers(TDSPlayer player)
        {
            return _loadMapOfOthersRightInfos.NeededAdminLevel.HasValue && _loadMapOfOthersRightInfos.NeededAdminLevel <= player.AdminLevel.Level
                || _loadMapOfOthersRightInfos.VipCanUse && player.Entity?.IsVip == true;

        }
    }
}