using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;

namespace TDS_Server.Handler.Server
{
    public class ServerStartHandler
    {
        public bool IsReadyForLogin 
            => _loadedServerBans;

        private bool _loadedServerBans;

        private readonly BansHandler _bansHandler;
        private readonly IModAPI _modAPI;

        public ServerStartHandler(BansHandler bansHandler, IModAPI modAPI, EventsHandler eventsHandler)
        {
            _bansHandler = bansHandler;
            _modAPI = modAPI;

            eventsHandler.LoadedServerBans += EventsHandler_LoadedServerBans;
        }

        private void EventsHandler_LoadedServerBans()
        {
            if (_loadedServerBans)
                return;

            _loadedServerBans = true;
            KickServerBannedPlayers();
        }

        private void KickServerBannedPlayers()
        {
            var players = _modAPI.Pool.GetAllModPlayers();
            foreach (var player in players)
            {
                var ban = _bansHandler.GetServerBan(null, player.IPAddress, player.Serial, player.SocialClubName, player.SocialClubId, null, false);
            }
        }
    }
}
