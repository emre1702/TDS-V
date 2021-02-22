using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces.GamemodesSystem;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler.Extensions;
using TDS.Server.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.RoundsHandlers
{
    public class ArenaRoundsHandler : RoundFightLobbyRoundsHandler, IArenaRoundsHandler
    {
        protected new IArena Lobby => (IArena)base.Lobby;

        public ArenaRoundsHandler(IArena lobby, IRoundFightLobbyEventsHandler events, IGamemodesProvider gamemodesProvider)
            : base(lobby, events, gamemodesProvider)
        {
        }

        protected override async ValueTask Events_RoundEnd()
        {
            await base.Events_RoundEnd().ConfigureAwait(false);

            if (Lobby.CurrentRoundEndReason.KillLoserTeam && Lobby.CurrentRoundEndReason.WinnerTeam is { })
            {
                var winnerTeam = Lobby.CurrentRoundEndReason.WinnerTeam;
                await Lobby.Players.DoInMain(player =>
                {
                    if (ShouldDieAtRoundEnd(player, winnerTeam))
                        player.Kill();
                }).ConfigureAwait(false);
            }
        }

        protected override async ValueTask Events_PlayerLeftAfter((ITDSPlayer Player, int HadLifes) data)
        {
            await base.Events_PlayerLeftAfter(data).ConfigureAwait(false);

            using var _ = await RoundStates.GetContext().ConfigureAwait(false);
            switch (RoundStates.CurrentState)
            {
                case NewMapChooseState _:
                case CountdownState _:
                    if (Lobby.Entity.LobbyRoundSettings.MixTeamsAfterRound)
                        await Lobby.Teams.BalanceCurrentTeams().ConfigureAwait(false);
                    break;
            }
        }

        protected override async Task SendPlayerRoundInfoOnJoin(ITDSPlayer player)
        {
            await base.SendPlayerRoundInfoOnJoin(player).ConfigureAwait(false);

            var mapVotingDataJson = Lobby.MapVoting.GetJson();
            NAPI.Task.RunSafe(() =>
            {
                if (mapVotingDataJson is { })
                    player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.LoadMapVoting, mapVotingDataJson);
            });
        }

        public override ValueTask<ITeam?> GetTimesUpWinnerTeam()
            => new ValueTask<ITeam?>(Lobby.Teams.GetTeamWithHighestHp());

        private bool ShouldDieAtRoundEnd(ITDSPlayer player, ITeam winnerTeam)
           => player.Lifes > 0 && winnerTeam is { } && player.Team != winnerTeam;

        public async Task<bool> PlayerJoinedRound(ITDSPlayer player)
        {
            if (player.Team is null)
                return false;

            if (!RoundStates.Started)
            {
                RoundStates.StartRound();
                return true;
            }

            using (await RoundStates.GetContext().ConfigureAwait(false))
            {
                if (CanJoinRound(player, out var freeze))
                {
                    SetPlayerReadyForRound(player, freeze);
                    if (!(RoundStates.CurrentState is CountdownState))
                        StartRoundForPlayer(player);
                    return true;
                }
            }

            return !await CheckForEnoughAliveAfterJoin().ConfigureAwait(false);
        }

        public override void SetPlayerReadyForRound(ITDSPlayer player, bool freeze)
        {
            if (player.Team != null && !player.Team.IsSpectator)
            {
                var spawndata = Lobby.MapHandler.GetMapRandomSpawnData(player.Team);
                if (spawndata is null)
                    return;
                player.Spawn(spawndata.ToVector3(), spawndata.Rotation);
            }
            else
            {
                player.Spawn(Lobby.Spectator.CurrentMapSpectatorPosition);
            }

            base.SetPlayerReadyForRound(player, freeze);
        }

        private bool CanJoinRound(ITDSPlayer player, out bool freeze)
        {
            var joinInRound = CurrentGamemode?.Rounds.CanJoinDuringRound(player, player.Team) == true && RoundStates.CurrentState is InRoundState;
            freeze = !joinInRound;
            return RoundStates.CurrentState is CountdownState || joinInRound;
        }
    }
}