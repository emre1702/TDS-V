using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class WeaponsLoadingState : RoundState
    {
        public override int Duration => 500;

        public WeaponsLoadingState(IRoundFightLobby lobby) : base(lobby)
        {
        }

        public override ValueTask SetCurrent()
        {
            Lobby.Events.TriggerWeaponsLoading();
            return default;
        }
    }
}
