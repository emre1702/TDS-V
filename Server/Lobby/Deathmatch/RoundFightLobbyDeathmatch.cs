using System.Threading.Tasks;
using TDS_Server.Core.Damagesystem;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class RoundFightLobbyDeathmatch : FightLobbyDeathmatch
    {
        public RoundFightLobbyDeathmatch(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, Damagesys damage, LangHelper langHelper)
            : base(lobby, events, damage, langHelper)
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
