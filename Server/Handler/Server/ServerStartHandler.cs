using GTANetworkAPI;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.Handler.Server
{
    public class ServerStartHandler
    {
        public TaskCompletionSource<bool> LoadingTask { get; } = new TaskCompletionSource<bool>();

        public bool LoadedServerBans
        {
            get => _loadedServerBans;
            set
            {
                _loadedServerBans = value;
                CheckIsServerLoaded();
            }
        }

        public bool LoadedChangelogs
        {
            get => _loadedChangelogs;
            set
            {
                _loadedChangelogs = value;
                CheckIsServerLoaded();
            }
        }

        private readonly BansHandler _bansHandler;

        private bool _loadedServerBans;
        private bool _loadedChangelogs;

        public ServerStartHandler(BansHandler bansHandler, EventsHandler eventsHandler)
        {
            _bansHandler = bansHandler;

            eventsHandler.LoadedServerBans += EventsHandler_LoadedServerBans;
            if (bansHandler.LoadedServerBans)
                LoadedServerBans = true;
        }

        private void EventsHandler_LoadedServerBans()
        {
            if (_loadedServerBans)
                return;

            _loadedServerBans = true;
            KickServerBannedPlayers();
        }

        private bool HandleBan(ITDSPlayer player, PlayerBans? ban)
        {
            if (ban is null)
                return true;

            string startstr = ban.StartTimestamp.ToString(DateTimeFormatInfo.InvariantInfo);
            string endstr = ban.EndTimestamp.HasValue ? ban.EndTimestamp.Value.ToString(DateTimeFormatInfo.InvariantInfo) : "never";

            var splittedReason = Utils.SplitPartsByLength($"Banned!\nName: {ban.Player?.Name ?? player.Name}\nAdmin: {ban.Admin.Name}\nReason: {ban.Reason}\nEnd: {endstr} UTC\nStart: {startstr} UTC", 90);

            NAPI.Task.RunSafe(() =>
            {
                foreach (var split in splittedReason)
                    player.SendNotification(split, true);
            });

            _ = new TDSTimer(() =>
            {
                if (!player.IsNull)
                {
                    NAPI.Task.RunSafe(() =>
                        player.Kick("Ban"));
                }
                    
            }, 3000, 1);

            return false;
        }

        private void KickServerBannedPlayers()
        {
            NAPI.Task.RunSafe(() =>
            {
                var players = NAPI.Pools.GetAllPlayers().OfType<ITDSPlayer>();
                foreach (var player in players)
                {
                    player.Init();
                    var ban = _bansHandler.GetServerBan(null, player.Address, player.Serial, player.SocialClubName, player.SocialClubId, null, false);
                    HandleBan(player, ban);
                }
            });
        }

        private void CheckIsServerLoaded()
        {
            if (_loadedServerBans && _loadedChangelogs)
            {
                LoadingTask.TrySetResult(true);
            }
        }
    }
}
