using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Account;

namespace TDS_Server.Handler.Server
{
    public class ServerStartHandler
    {
        public bool IsReadyForLogin 
            => LoadedServerBans;

        public bool LoadedServerBans 
        { 
            get => _loadedServerBans; 
            set 
            { 
                if (_loadedServerBans == value)
                    return;

                _loadedServerBans = value; 
                KickServerBannedPlayers();
            }
        }

        private bool _loadedServerBans;

        private readonly BansHandler _bansHandler;
        private readonly IModAPI _modAPI;

        public ServerStartHandler(BansHandler bansHandler, IModAPI modAPI)
        {
            _bansHandler = bansHandler;
            _modAPI = modAPI;
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
