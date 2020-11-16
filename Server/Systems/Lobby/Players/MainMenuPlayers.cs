using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.LobbySystem.Players
{
    public class MainMenuPlayers : BaseLobbyPlayers
    {
        public MainMenuPlayers(IMainMenu lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            var worked = await base.AddPlayer(player, 0).ConfigureAwait(false);
            if (!worked)
                return false;

            NAPI.Task.RunSafe(() =>
            {
                player.SetInvincible(true);
                player.SetInvisible(true);
            });
            return true;
        }

        protected override void InformAboutHowToLeaveLobby(ITDSPlayer player)
        {
        }
    }
}
