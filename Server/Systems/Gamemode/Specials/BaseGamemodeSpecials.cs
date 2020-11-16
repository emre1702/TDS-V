using TDS.Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.GamemodesSystem.Specials
{
    public class BaseGamemodeSpecials : IBaseGamemodeSpecials
    {
        protected IRoundFightLobby Lobby { get; private set; }

        public BaseGamemodeSpecials(IRoundFightLobby lobby)
            => Lobby = lobby;

        internal virtual void AddEvents(IRoundFightLobbyEventsHandler events)
        {
        }

        internal virtual void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
        }
    }
}
