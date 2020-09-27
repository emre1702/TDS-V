using System.Threading.Tasks;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.RoundsHandlers
{
    internal class GangActionLobbyRoundsHandler : RoundFightLobbyRoundsHandler
    {
        protected new GangActionLobby Lobby => (GangActionLobby)base.Lobby;

        public GangActionLobbyRoundsHandler(GangActionLobby lobby) : base(lobby)
        {
            lobby.Events.RoundClear += RoundClear;
        }

        public virtual async ValueTask RoundClear()
        {
            RoundStates.Stop();
            await Lobby.Remove();
        }

        //Todo: Add GetWinnerTeam
    }
}
