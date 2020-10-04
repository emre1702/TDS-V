using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.RoundsHandlers
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
            await base.Events_RoundEnd();

            if (Lobby.CurrentRoundEndReason.KillLoserTeam && Lobby.CurrentRoundEndReason.WinnerTeam is { })
            {
                var winnerTeam = Lobby.CurrentRoundEndReason.WinnerTeam;
                await Lobby.Players.DoInMain(player =>
                {
                    if (ShouldDieAtRoundEnd(player, winnerTeam))
                        player.Kill();
                });
            }
        }

        protected override async ValueTask Events_PlayerLeftAfter((ITDSPlayer Player, int HadLifes) data)
        {
            await base.Events_PlayerLeftAfter(data);

            using var _ = await RoundStates.GetContext();
            switch (RoundStates.CurrentState)
            {
                case NewMapChooseState _:
                case CountdownState _:
                    if (Lobby.Entity.LobbyRoundSettings.MixTeamsAfterRound)
                        await Lobby.Teams.BalanceCurrentTeams();
                    break;
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
                RoundStates.Start();
                return true;
            }

            using var _ = await RoundStates.GetContext();
            var joinInRound = CurrentGamemode?.Rounds.CanJoinDuringRound(player, player.Team) == true && RoundStates.CurrentState is InRoundState;
            if (RoundStates.CurrentState is CountdownState || joinInRound)
            {
                bool freeze = !joinInRound;
                SetPlayerReadyForRound(player, freeze);
                if (joinInRound)
                    StartRoundForPlayer(player);
                return true;
            }

            return !(await CheckForEnoughAliveAfterJoin());
        }

        public override void SetPlayerReadyForRound(ITDSPlayer player, bool freeze)
        {
            if (player.Team != null && !player.Team.IsSpectator)
            {
                var spawndata = Lobby.MapHandler.GetMapRandomSpawnData(player.Team);
                if (spawndata is null)
                    return;
                NAPI.Task.Run(() => player.Spawn(spawndata.ToVector3(), spawndata.Rotation));
            }
            else
            {
                NAPI.Task.Run(() => player.Spawn(Lobby.Spectator.CurrentMapSpectatorPosition));
            }

            base.SetPlayerReadyForRound(player, freeze);
        }
    }
}
