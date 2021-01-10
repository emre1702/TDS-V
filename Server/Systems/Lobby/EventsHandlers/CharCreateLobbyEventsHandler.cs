using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.EventsHandlers
{
    internal class CharCreateLobbyEventsHandler : BaseLobbyEventsHandler
    {
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly RemoteBrowserEventsHandler _remoteBrowserEventsHandler;

        public CharCreateLobbyEventsHandler(ICharCreateLobby lobby, EventsHandler eventsHandler, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(lobby, eventsHandler, loggingHandler)
        {
            _lobbiesHandler = lobbiesHandler;
            _remoteBrowserEventsHandler = remoteBrowserEventsHandler;

            _remoteBrowserEventsHandler.Add(ToServerEvent.CancelCharCreateData, Cancel, player => player.Lobby == lobby);
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);

            _remoteBrowserEventsHandler.Remove(ToServerEvent.CancelCharCreateData, Cancel);
        }

        internal async ValueTask<object?> Cancel(RemoteBrowserEventArgs args)
        {
            await _lobbiesHandler.MainMenu.Players.AddPlayer(args.Player, 0).ConfigureAwait(false);
            return null;
        }
    }
}