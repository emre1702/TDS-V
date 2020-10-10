using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Spectator;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Spectator
{
    public class FightLobbySpectator : IFightLobbySpectator
    {
        protected IFightLobby Lobby { get; }

        public FightLobbySpectator(IFightLobby lobby)
        {
            Lobby = lobby;
        }

        public void SetPlayerInSpectateMode(ITDSPlayer player)
        {
            player.DeathSpawnTimer?.Kill();
            player.DeathSpawnTimer = new TDSTimer(async () =>
            {
                NAPI.Task.Run(() =>
                    player.TriggerEvent(ToClientEvent.PlayerSpectateMode));
                await EnsurePlayerSpectatesAnyone(player).ConfigureAwait(false);
            }, (uint)Lobby.Entity.FightSettings.SpawnAgainAfterDeathMs);
        }

        public async Task EnsurePlayerSpectatesAnyone(ITDSPlayer player)
        {
            if (player.Spectates is { } && player.Spectates != player)
                return;

            await SpectateNext(player, true).ConfigureAwait(false);

            if (player.Spectates is null || player.Spectates == player)
                await SpectateOtherAllTeams(player).ConfigureAwait(false);
        }

        public async ValueTask SpectateNext(ITDSPlayer player, bool forward)
        {
            if (player.Lifes > 0)
                return;

            if (player.Team is null || player.Team.IsSpectator)
                await SpectateOtherAllTeams(player, forward).ConfigureAwait(false);
            else
                SpectateOtherSameTeam(player, forward);
        }

        public virtual async Task SpectateOtherAllTeams(ITDSPlayer player, bool spectateNext = true)
        {
            var currentlySpectating = player.Spectates ?? player;
            ITDSPlayer? nextPlayer;
            if (spectateNext)
                nextPlayer = await GetNextSpectatePlayerInAllTeams(currentlySpectating).ConfigureAwait(false);
            else
                nextPlayer = await GetPreviousSpectatePlayerInAllTeams(currentlySpectating).ConfigureAwait(false);
            nextPlayer ??= currentlySpectating;

            player.Spectates = nextPlayer;
        }

        public virtual void SpectateOtherSameTeam(ITDSPlayer player, bool spectateNext = true, bool ignoreSource = false)
        {
            var currentlySpectating = player.Spectates ?? player;
            ITDSPlayer? nextPlayer;
            if (spectateNext)
                nextPlayer = GetNextSpectatePlayerInSameTeam(currentlySpectating);
            else
                nextPlayer = GetPreviousSpectatePlayerInSameTeam(currentlySpectating);
            nextPlayer ??= player;

            if (ignoreSource && player == nextPlayer)
                return;
            player.Spectates = nextPlayer;
        }

        private async Task<ITDSPlayer?> GetNextSpectatePlayerInAllTeams(ITDSPlayer start)
        {
            var nextIndexInSameTeam = GetNextSpectatePlayerIndexInSameTeam(start);
            // nextIndexInSameTeam == 0   =>   there is no next player in same team, only previous players
            // But then we should have to use next team instead.
            if (nextIndexInSameTeam.HasValue && nextIndexInSameTeam != 0)
                return start.Team?.Players.GetSpectatableAtIndex(nextIndexInSameTeam.Value);

            var nextTeam = await Lobby.Teams.GetNextTeamWithSpectatablePlayers(start.Team).ConfigureAwait(false);
            return nextTeam?.Players.GetSpectatableAtIndex(0);
        }

        private ITDSPlayer? GetNextSpectatePlayerInSameTeam(ITDSPlayer start)
        {
            var nextIndex = GetNextSpectatePlayerIndexInSameTeam(start);
            if (nextIndex is null)
                return null;
            return start.Team?.Players.GetSpectatableAtIndex(nextIndex.Value);
        }

        private int? GetNextSpectatePlayerIndexInSameTeam(ITDSPlayer start)
        {
            if (start.Team is null || start.Team.IsSpectator)
                return null;
            if (start.Team.Players.AmountSpectatable == 0)
                return null;
            var playerIndexToTake = start.Team.Players.GetSpectatableIndex(start) + 1;
            if (playerIndexToTake >= start.Team.Players.AmountSpectatable)
                playerIndexToTake = 0;
            return playerIndexToTake;
        }

        private async Task<ITDSPlayer?> GetPreviousSpectatePlayerInAllTeams(ITDSPlayer start)
        {
            var previousIndexInSameTeam = GetPreviousSpectatePlayerIndexInSameTeam(start);
            // nextIndexInSameTeam == endIndex   =>   there is no previous player in same team, only next players
            // But then we should have to use previous team instead.
            if (previousIndexInSameTeam.HasValue && previousIndexInSameTeam != start.Team!.Players.AmountSpectatable - 1)
                return start.Team.Players.GetSpectatableAtIndex(previousIndexInSameTeam.Value);

            var nextTeam = await Lobby.Teams.GetPreviousTeamWithSpectatablePlayers(start.Team).ConfigureAwait(false);
            return nextTeam?.Players.GetSpectatableAtIndex(0);
        }

        private ITDSPlayer? GetPreviousSpectatePlayerInSameTeam(ITDSPlayer start)
        {
            var previousIndex = GetPreviousSpectatePlayerIndexInSameTeam(start);
            if (previousIndex is null)
                return null;
            return start.Team?.Players.GetSpectatableAtIndex(previousIndex.Value);
        }

        private int? GetPreviousSpectatePlayerIndexInSameTeam(ITDSPlayer start)
        {
            if (start.Team is null || start.Team.IsSpectator)
                return null;
            if (start.Team.Players.AmountSpectatable == 0)
                return null;
            var playerIndexToTake = start.Team.Players.GetSpectatableIndex(start) - 1;
            if (playerIndexToTake < 0)
                playerIndexToTake = start.Team.Players.AmountSpectatable - 1;
            return playerIndexToTake;
        }
    }
}
