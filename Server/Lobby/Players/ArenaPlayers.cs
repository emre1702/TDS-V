using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.CustomLobby;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Players
{
    public class ArenaPlayers : RoundFightLobbyPlayers
    {
        public new IArena Lobby => (IArena)base.Lobby;

        public ArenaPlayers(IArena arena, IRoundFightLobbyEventsHandler events)
            : base(arena, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            var worked = await base.AddPlayer(player, teamIndex);
            if (!worked)
                return false;

            var spawnPos = Lobby.CurrentMap?.LimitInfo?.Center?.ToVector3()?.AddToZ(10) ?? Lobby.MapHandler.SpawnPoint;
            var teamChoiceData = await Lobby.Teams.Do(teams =>
                teams.Select(t => new TeamChoiceMenuTeamData(t.Entity.Name, t.Entity.ColorR, t.Entity.ColorG, t.Entity.ColorB)));
            var teamChoiceDataJson = Serializer.ToBrowser(teamChoiceData);
            new TDSTimer(() => Lobby.Spectator.SpectateOtherAllTeams(player), 1000, 1);

            NAPI.Task.Run(() =>
            {
                player.Spawn(spawnPos);
                StartTeamChoice(player, teamChoiceDataJson);
            });

            return true;
        }

        private void StartTeamChoice(ITDSPlayer player, string teamChoiceDataJson)
            => player.TriggerEvent(ToClientEvent.SyncTeamChoiceMenuData, teamChoiceDataJson, Lobby.Entity.LobbyRoundSettings.MixTeamsAfterRound);

        internal void ChooseTeam(ITDSPlayer player, int teamIndex)
        {
            player.CurrentRoundStats = new RoundStatsDto(player);

            if (teamIndex != 0)
            {
                Lobby.Spectator.SpectateOtherSameTeam(player);
                var team = Lobby.Entity.LobbyRoundSettings.MixTeamsAfterRound ? GetTeamWithFewestPlayer() : Teams[teamIndex];
                Lobby.Teams.SetPlayerTeam(player, team);
            }
        }

        protected override void Events_PlayersPreparation()
        {
            base.Events_PlayersPreparation();
            //Todo: Implement this but without countdown, only spawn:
            //SetAllPlayersInCountdown();
        }

        protected override void Events_Countdown()
        {
            base.Events_Countdown();
            //Todo: Implement this but only countdown:
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

        protected override async Task SendPlayerRoundInfoOnJoin(ITDSPlayer player)
        {
            await base.SendPlayerRoundInfoOnJoin(player);

            var mapVotingDataJson = Lobby.MapVoting.GetJson();
            NAPI.Task.Run(() =>
            {
                if (mapVotingDataJson is { })
                    player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.LoadMapVoting, mapVotingDataJson);
            });
        }

        private bool ShouldDieAtRoundEnd(ITDSPlayer player, ITeam winnerTeam)
            => player.Lifes > 0 && winnerTeam is { } && player.Team == winnerTeam;
    }
}
