using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.EventsHandlers
{
    internal class CharCreateLobbyEventsHandler : BaseLobbyEventsHandler
    {
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly RemoteBrowserEventsHandler _remoteBrowserEventsHandler;

        private static int _addedCancelEventCount = 0;
        private static object _addedCancelEventCountLock = new object();

        public CharCreateLobbyEventsHandler(ICharCreateLobby lobby, EventsHandler eventsHandler, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(lobby, eventsHandler, loggingHandler)
        {
            _lobbiesHandler = lobbiesHandler;
            _remoteBrowserEventsHandler = remoteBrowserEventsHandler;

            lock (_addedCancelEventCountLock)
            {
                if (++_addedCancelEventCount == 1)
                    _remoteBrowserEventsHandler.AddAsyncEvent(ToServerEvent.CancelCharCreateData, Cancel);
            }
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);

            lock (_addedCancelEventCountLock)
            {
                if (--_addedCancelEventCount == 0)
                    _remoteBrowserEventsHandler.RemoveAsyncEvent(ToServerEvent.CancelCharCreateData);
            }
        }

        internal async Task<object?> Cancel(ITDSPlayer player, ArraySegment<object> _)
        {
            if (!(player.Lobby is ICharCreateLobby))
                return null;

            await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0).ConfigureAwait(false);
            return null;
        }
    }
}