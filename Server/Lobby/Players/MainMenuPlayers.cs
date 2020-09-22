using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Players
{
    public class MainMenuPlayers : BaseLobbyPlayers
    {
        public MainMenuPlayers(IBaseLobby lobby, IBaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams, BaseLobbyBansHandler bans)
            : base(lobby, events, teams, bans)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            var worked = await base.AddPlayer(player, 0);
            if (!worked)
                return false;

            NAPI.Task.Run(() =>
            {
                player.SetInvincible(true);
            });
            return true;
        }

        protected override void InformAboutHowToLeaveLobby(ITDSPlayer player)
        {
        }
    }
}
