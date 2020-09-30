using System.Threading.Tasks;
using TDS_Server.Core.Damagesystem;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class RoundFightLobbyDeathmatch : FightLobbyDeathmatch, IRoundFightLobbyDeathmatch
    {
        protected new IRoundFightLobby Lobby => (IRoundFightLobby)base.Lobby;

        public RoundFightLobbyDeathmatch(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, Damagesys damage, LangHelper langHelper)
            : base(lobby, events, damage, langHelper)
        {
            events.RoundClear += RoundClear;
        }

        public async Task RemovePlayerFromAlive(ITDSPlayer player)
        {
            player.Team?.RemoveAlivePlayer(player);
            await Lobby.Spectator.SetPlayerCantBeSpectatedAnymore(player);
        }

        private ValueTask RoundClear()
        {
            Damage.Clear();
            return default;
        }
    }
}
