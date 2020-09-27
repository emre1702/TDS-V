using System.Threading.Tasks;
using TDS_Server.Core.Damagesystem;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Spectator;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class RoundFightLobbyDeathmatch : FightLobbyDeathmatch
    {
        public RoundFightLobbyDeathmatch(IRoundFightLobbyEventsHandler events, IRoundFightLobby lobby, Damagesys damage, LangHelper langHelper, IBaseLobbyPlayers players, FightLobbySpectator spectator)
            : base(events, lobby, damage, langHelper, players, spectator)
        {
            events.RoundClear += RoundClear;
        }

        private ValueTask RoundClear()
        {
            Damage.Clear();
            return default;
        }
    }
}
