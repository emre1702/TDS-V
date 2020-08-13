using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Data;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Player
{
    public class ConnectedHandler
    {
        #region Private Fields

        private readonly BansHandler _bansHandler;
        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly EventsHandler _eventsHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly IModAPI _modAPI;

        #endregion Private Fields

        #region Public Constructors

        public ConnectedHandler(
            BansHandler bansHandler,
            EventsHandler eventsHandler,
            LobbiesHandler lobbiesHandler,
            DatabasePlayerHelper databasePlayerHelper,
            IModAPI modAPI)
        {
            _modAPI = modAPI;
            _bansHandler = bansHandler;
            _eventsHandler = eventsHandler;
            _lobbiesHandler = lobbiesHandler;
            _databasePlayerHelper = databasePlayerHelper;

            _eventsHandler.PlayerConnected += PlayerConnected;
        }

        #endregion Public Constructors

        #region Private Methods

        private async void PlayerConnected(IPlayer modPlayer)
        {
            if (modPlayer is null)
                return;

            modPlayer.Position = new Position(0, 0, 1000).Around(10);
            modPlayer.Freeze(true);

            var ban = await _bansHandler.GetBan(_lobbiesHandler.MainMenu.Id, null, modPlayer.Address, modPlayer.Serial, modPlayer.SocialClubName,
                modPlayer.SocialClubId, false);

            if (ban is { })
            {
                AltAsync.Do(()
                    => Utils.HandleBan(modPlayer, ban));
                return;
            }

            var playerIdName = await _databasePlayerHelper.GetPlayerIdName(modPlayer);
            if (playerIdName is null)
            {
                AltAsync.Do(()
                    => modPlayer.SendEvent(ToClientEvent.StartRegisterLogin, modPlayer.SocialClubName, false));
                return;
            }

            ban = await _bansHandler.GetBan(_lobbiesHandler.MainMenu.Id, playerIdName.Id);
            if (ban is { })
            {
                AltAsync.Do(()
                    => Utils.HandleBan(modPlayer, ban));
                return;
            }

            AltAsync.Do(()
                => modPlayer.SendEvent(ToClientEvent.StartRegisterLogin, playerIdName.Name, true));
        }

        #endregion Private Methods
    }
}
