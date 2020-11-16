using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class PlayerPreparationState : RoundState
    {
        public override int Duration => 2 * 1000;

        public PlayerPreparationState(IRoundFightLobby lobby) : base(lobby)
        {
        }

        public override ValueTask SetCurrent()
        {
            Lobby.Events.TriggerPlayersPreparation();
            return default;
        }
    }
}
