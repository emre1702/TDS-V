using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Events.Mod;

namespace TDS_Server.Core.Player.Join
{
    class ConnectedHandler
    {
        private IModAPI _api;
        private BansHandler _bansHandler;
        private EventsHandler _eventsHandler;

        public ConnectedHandler(
            IModAPI api,
            BansHandler bansHandler,
            EventsHandler eventsHandler)
        {
            _api = api;
            _bansHandler = bansHandler;
            _eventsHandler = eventsHandler;

            _eventsHandler.PlayerConnected += PlayerConnected;
        }

        private async void PlayerConnected(ITDSPlayer player)
        {
            player.ModPlayer.Position = new Position3D(0, 0, 1000).Around(10);



            client.Position = new Vector3(0, 0, 1000).Around(10);
            Workaround.FreezePlayer(client, true);

            var ban = await _bansManager.GetBan(LobbyManager.MainMenu.Id, null, client.Address, client.Serial, client.SocialClubName, client.SocialClubId, false).ConfigureAwait(true);
            if (!HandlePlayerBan(client, ban))
                return;

            using var dbContext = new TDSDbContext();
            var playerIDName = await dbContext.Players.Where(p => p.Name == client.Name || p.SCName == client.SocialClubName).Select(p => new { p.Id, p.Name })
                .FirstOrDefaultAsync()
                .ConfigureAwait(true);
            if (playerIDName is null)
            {
                NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.StartRegisterLogin, client.SocialClubName, false);
                return;
            }

            ban = await _bansManager.GetBan(LobbyManager.MainMenu.Id, playerIDName.Id).ConfigureAwait(true);
            if (!HandlePlayerBan(client, ban))
                return;

            NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.StartRegisterLogin, playerIDName.Name, true);
        }
    }
}
