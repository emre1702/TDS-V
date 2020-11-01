﻿using GTANetworkAPI;
using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Shared.Default;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.Handler.PlayerHandlers
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

                await NAPI.Task.RunWait(player.Init);

                NAPI.Task.RunSafe(() =>
                {
                    player.Position = new Vector3(0, 0, 1000).Around(10);
                    player.Freeze(true);
                });

                var ban = await _bansHandler.GetBan(_lobbiesHandler.MainMenu.Entity.Id, null, player.Address, player.Serial, player.SocialClubName,
                    player.SocialClubId, false).ConfigureAwait(false);

                if (ban is { })
                {
                    NAPI.Task.RunSafe(()
                        => Utils.HandleBan(player, ban));
                    return;
                }

                var playerIdName = await _databasePlayerHelper.GetPlayerIdName(player).ConfigureAwait(false);
                if (playerIdName is null)
                {
                    NAPI.Task.RunSafe(()
                        => player.TriggerEvent(ToClientEvent.StartRegisterLogin, player.SocialClubName, false));
                    return;
                }

                ban = await _bansHandler.GetBan(_lobbiesHandler.MainMenu.Entity.Id, playerIdName.Id).ConfigureAwait(false);
                if (ban is { })
                {
                    NAPI.Task.RunSafe(()
                        => Utils.HandleBan(player, ban));
                    return;
                }

                NAPI.Task.RunSafe(()
                    => player.TriggerEvent(ToClientEvent.StartRegisterLogin, playerIdName.Name, true));
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }
    }
}
