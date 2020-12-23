using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Utility;
using TDS.Server.Handler.Account;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Shared.Default;
using TDS.Server.Handler.Extensions;
using System.Threading.Tasks;
using TDS.Server.Data.Models;

namespace TDS.Server.Handler.PlayerHandlers
{
    public class ConnectedHandler
    {
        private readonly BansHandler _bansHandler;
        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly EventsHandler _eventsHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;

        public ConnectedHandler(
            BansHandler bansHandler,
            EventsHandler eventsHandler,
            LobbiesHandler lobbiesHandler,
            DatabasePlayerHelper databasePlayerHelper,
            ILoggingHandler loggingHandler)
        {
            _bansHandler = bansHandler;
            _eventsHandler = eventsHandler;
            _lobbiesHandler = lobbiesHandler;
            _databasePlayerHelper = databasePlayerHelper;
            _loggingHandler = loggingHandler;

            _eventsHandler.PlayerConnected += PlayerConnected;
        }

        private async void PlayerConnected(ITDSPlayer player)
        {
            try
            {
                if (player is null)
                    return;

                await NAPI.Task.RunWait(player.Init).ConfigureAwait(false);
                Console.WriteLine($"Player connected | Name: {player.Name} | ScName: {player.SocialClubName} | ScId: {player.SocialClubId} | IP: {player.Address}");
                SpawnPlayerInWorld(player);

                if (await CheckIsBanned(player).ConfigureAwait(false))
                    return;

                var playerIdName = await GetRegisteredPlayerIdAndName(player).ConfigureAwait(false);
                if (playerIdName is null)
                    return;

                await CheckIsAccountBanned(player, playerIdName).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        private void SpawnPlayerInWorld(ITDSPlayer player)
        {
            NAPI.Task.RunSafe(() =>
            {
                player.Position = new Vector3(0, 0, 1000).Around(10);
                player.Freeze(true);
            });
        }

        private async Task<bool> CheckIsBanned(ITDSPlayer player)
        {
            var ban = await _bansHandler.GetBan(_lobbiesHandler.MainMenu.Entity.Id, null, player.Address, player.Serial, player.SocialClubName,
                    player.SocialClubId, false).ConfigureAwait(false);

            if (ban is { })
            {
                NAPI.Task.RunSafe(()
                    => Utils.HandleBan(player, ban));
                return true;
            }

            return false;
        }

        private async Task<DatabasePlayerIdName?> GetRegisteredPlayerIdAndName(ITDSPlayer player)
        {
            var playerIdName = await _databasePlayerHelper.GetPlayerIdName(player).ConfigureAwait(false);
            return playerIdName;
        }

        private async Task<bool> CheckIsAccountBanned(ITDSPlayer player, DatabasePlayerIdName playerIdName)
        {
            var ban = await _bansHandler.GetBan(_lobbiesHandler.MainMenu.Entity.Id, playerIdName.Id).ConfigureAwait(false);
            if (ban is { })
            {
                NAPI.Task.RunSafe(()
                    => Utils.HandleBan(player, ban));
                return true;
            }
            return false;
        }

    }
}
