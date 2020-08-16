using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
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

        #endregion Private Fields

        #region Public Constructors

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

            AltAsync.OnPlayerConnect += PlayerConnected;
        }

        #endregion Public Constructors

        #region Private Methods

        private async Task PlayerConnected(IPlayer modPlayer, string reason)
        {
            var player = (ITDSPlayer)modPlayer;
            if (player is null)
                return;

            player.Position = new Position(0, 0, 1000).Around(10);
            player.Freeze(true);

            var ban = await _bansHandler.GetBan(_lobbiesHandler.MainMenu.Id, null, player.Ip, player.SocialClubId, 
                player.HardwareIdHash, player.HardwareIdExHash, false);

            if (ban is { })
            {
                await AltAsync.Do(()
                    => Utils.HandleBan(player, ban));
                return;
            }

            var playerIdName = await _databasePlayerHelper.GetPlayerIdName(player);
            if (playerIdName is null)
            {
                await AltAsync.Do(()
                    => player.SendEvent(ToClientEvent.StartRegisterLogin, player.Name, false));
                return;
            }

            ban = await _bansHandler.GetBan(_lobbiesHandler.MainMenu.Id, playerIdName.Id);
            if (ban is { })
            {
                await AltAsync.Do(()
                    => Utils.HandleBan(player, ban));
                return;
            }

            await AltAsync.Do(()
                => player.SendEvent(ToClientEvent.StartRegisterLogin, playerIdName.Name, true));
        }

        #endregion Private Methods
    }
}
