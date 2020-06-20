using System.Globalization;
using System.Numerics;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Server
{
    public class ServerStartHandler
    {
        #region Private Fields

        private readonly BansHandler _bansHandler;

        private readonly IModAPI _modAPI;

        private bool _loadedServerBans;

        #endregion Private Fields

        #region Public Constructors

        public ServerStartHandler(BansHandler bansHandler, IModAPI modAPI, EventsHandler eventsHandler)
        {
            _bansHandler = bansHandler;
            _modAPI = modAPI;

            eventsHandler.LoadedServerBans += EventsHandler_LoadedServerBans;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsReadyForLogin
                                            => _loadedServerBans;

        #endregion Public Properties

        #region Private Methods

        private void EventsHandler_LoadedServerBans()
        {
            if (_loadedServerBans)
                return;

            _loadedServerBans = true;
            KickServerBannedPlayers();
        }

        private bool HandleBan(IPlayer player, PlayerBans? ban)
        {
            if (ban is null)
                return true;

            string startstr = ban.StartTimestamp.ToString(DateTimeFormatInfo.InvariantInfo);
            string endstr = ban.EndTimestamp.HasValue ? ban.EndTimestamp.Value.ToString(DateTimeFormatInfo.InvariantInfo) : "never";

            var splittedReason = Utils.SplitPartsByLength($"Banned!\nName: {ban.Player?.Name ?? player.Name}\nAdmin: {ban.Admin.Name}\nReason: {ban.Reason}\nEnd: {endstr} UTC\nStart: {startstr} UTC", 90);

            foreach (var split in splittedReason)
                player.SendNotification(split, true);

            _ = new TDSTimer(() =>
            {
                if (!player.IsNull)
                    player.Kick("Ban");
            }, 3000, 1);

            return false;
        }

        private void KickServerBannedPlayers()
        {
            var players = _modAPI.Pool.Players.All;
            foreach (var player in players)
            {
                var ban = _bansHandler.GetServerBan(null, player.Address, player.Serial, player.SocialClubName, player.SocialClubId, null, false);
                HandleBan(player, ban);
            }
        }

        #endregion Private Methods
    }
}
