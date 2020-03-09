using TDS_Server.Data.Interfaces.ModAPI;

namespace TDS_Server.Core.Player.Join
{
    class ConnectedHandler
    {
        private BansHandler _bansHandler;

        public ConnectedHandler(
            IModAPI api,
            BansHandler bansHandler)
        {
            _bansHandler = bansHandler;

            api.Events.PlayerConnected += PlayerConnected;
        }

        private async void PlayerConnected(IPlayer player)
        {
            while (_bansManager is null)
                await Task.Delay(1000).ConfigureAwait(true);

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
