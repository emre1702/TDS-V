using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class RoundFightLobbyDeathmatch : FightLobbyDeathmatch, IRoundFightLobbyDeathmatch
    {
        protected new IRoundFightLobby Lobby => (IRoundFightLobby)base.Lobby;

        public RoundFightLobbyDeathmatch(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, IDamagesys damage, LangHelper langHelper)
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

        public override async Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            var lifes = player.Lifes;
            await base.OnPlayerDeath(player, killer, weapon);

            if (lifes == 1 && player.Lifes == 0)
            {
                await RemovePlayerFromAlive(player);
                await Lobby.Rounds.CheckForEnoughAlive();
            }
            else if (lifes > 0)
                Lobby.Players.RespawnPlayer(player);
        }
    }
}
