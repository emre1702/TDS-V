using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Player
{
    class ConnectedHandler
    {
        private readonly BansHandler _bansHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly DatabasePlayerHelper _databasePlayerHelper;

        public ConnectedHandler(
            BansHandler bansHandler,
            EventsHandler eventsHandler,
            LobbiesHandler lobbiesHandler,
            DatabasePlayerHelper databasePlayerHelper)
        {
            _bansHandler = bansHandler;
            _eventsHandler = eventsHandler;
            _lobbiesHandler = lobbiesHandler;
            _databasePlayerHelper = databasePlayerHelper;

            _eventsHandler.PlayerConnected += PlayerConnected;
        }

        private async void PlayerConnected(ITDSPlayer player)
        {
            if (player.ModPlayer is null)
                return;

            player.ModPlayer.Position = new Position3D(0, 0, 1000).Around(10);
            player.ModPlayer.Freeze(true);

            var ban = await _bansHandler.GetBan(_lobbiesHandler.MainMenu.Id, null, player.ModPlayer.IPAddress, player.ModPlayer.Serial, player.ModPlayer.SocialClubName,
                player.ModPlayer.SocialClubId, false);
            if (!player.HandleBan(ban))
                return;

            var playerIdName = await _databasePlayerHelper.GetPlayerIdName(player);
            if (playerIdName is null)
            {
                player.SendEvent(ToClientEvent.StartRegisterLogin, player.ModPlayer.SocialClubName, false);
                return;
            }

            ban = await _bansHandler.GetBan(_lobbiesHandler.MainMenu.Id, playerIdName.Id);
            if (!player.HandleBan(ban))
                return;

            player.SendEvent(ToClientEvent.StartRegisterLogin, playerIdName.Name, true);
        }
    }
}
