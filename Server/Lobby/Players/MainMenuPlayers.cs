using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.LobbySystem.Players
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
