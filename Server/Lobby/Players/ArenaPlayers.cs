using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Players
{
    public class ArenaPlayers : RoundFightLobbyPlayers
    {
        public ArenaPlayers(Arena arena, IRoundFightLobbyEventsHandler events, ArenaTeamsHandler teams, BaseLobbyBansHandler bans)
            : base(arena, events, teams, bans)
        {
        }

        protected override void Events_PlayersPreparation()
        {
            base.Events_PlayersPreparation();
            //Todo: Implement this but without countdown, only spawn:
            //SetAllPlayersInCountdown();
        }

        protected override async ValueTask Events_RoundEnd()
        {
            await base.Events_RoundEnd();

            if (Lobby.CurrentRoundEndReason.KillLoserTeam && Lobby.CurrentRoundEndReason.WinnerTeam is { })
            {
                var winnerTeam = Lobby.CurrentRoundEndReason.WinnerTeam;
                await DoInMain(player =>
                {
                    if (ShouldDieAtRoundEnd(player, winnerTeam))
                        player.Kill();
                });
            }
        }

        private bool ShouldDieAtRoundEnd(ITDSPlayer player, ITeam winnerTeam)
            => player.Lifes > 0 && winnerTeam is { } && player.Team == winnerTeam;
    }
}
