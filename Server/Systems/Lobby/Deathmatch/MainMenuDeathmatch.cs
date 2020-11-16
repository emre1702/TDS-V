using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.LobbySystem.Deathmatch
{
    public class MainMenuDeathmatch : BaseLobbyDeathmatch
    {
        public MainMenuDeathmatch(IMainMenu lobby, IBaseLobbyEventsHandler events) : base(lobby, events)
        {
        }

        public override async Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            await base.OnPlayerDeath(player, killer, weapon).ConfigureAwait(false);

            NAPI.Task.RunSafe(() =>
            {
                player.Spawn(Lobby.MapHandler.SpawnPoint, Lobby.MapHandler.SpawnRotation);
                player.Freeze(true);
            });
        }
    }
}
