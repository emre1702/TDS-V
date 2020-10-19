using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class DamageTestLobbyDeathmatch : FightLobbyDeathmatch
    {
        public DamageTestLobbyDeathmatch(IDamageTestLobby lobby, IFightLobbyEventsHandler events, IDamagesys damage, LangHelper langHelper)
            : base(lobby, events, damage, langHelper)
        {
        }

        public override async Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            await base.OnPlayerDeath(player, killer, weapon).ConfigureAwait(false);

            NAPI.Task.Run(() =>
            {
                player.Spawn(Lobby.MapHandler.SpawnPoint, Lobby.MapHandler.SpawnRotation);
            });
        }
    }
}
