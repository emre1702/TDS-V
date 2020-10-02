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
                await EnsurePlayerSpectatesAnyone(player);
            }, (uint)Lobby.Entity.FightSettings.SpawnAgainAfterDeathMs);
        }

        public async Task EnsurePlayerSpectatesAnyone(ITDSPlayer player)
        {
            if (player.Spectates is { } && player.Spectates != player)
                return;

            await SpectateNext(player, true);

            if (player.Spectates is null || player.Spectates == player)
                await SpectateOtherAllTeams(player);
        }

        public async ValueTask SpectateNext(ITDSPlayer player, bool forward)
        {
            if (player.Lifes > 0)
                return;

            if (player.Team is null || player.Team.IsSpectator)
                await SpectateOtherAllTeams(player, forward);
            else
                SpectateOtherSameTeam(player, forward);
        }

        public virtual async Task SpectateOtherAllTeams(ITDSPlayer player, bool spectateNext = true)
        {
            var currentlySpectating = player.Spectates ?? player;
            ITDSPlayer? nextPlayer;
            if (spectateNext)
                nextPlayer = await GetNextSpectatePlayerInAllTeams(currentlySpectating);
            else
                nextPlayer = await GetPreviousSpectatePlayerInAllTeams(currentlySpectating);
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
            var teamIndex = start.Team?.Entity.Index ?? 0;
            if (teamIndex == 0)
                ++teamIndex;
            var teamlist = (await Lobby.Teams.GetTeam(teamIndex)).SpectateablePlayers;
            if (teamlist is null)
                return null;
            var charIndex = teamlist.IndexOf(start) + 1;
            if (teamlist.Count == 0 || charIndex >= teamlist.Count - 1)
            {
                var team = await Lobby.Teams.GetNextNonSpectatorTeamWithPlayers(start.Team);
                if (team is null)
                    return null;
                teamlist = team.SpectateablePlayers;
                if (teamlist is null)
                    return null;
                charIndex = 0;
            }
            return teamlist[charIndex];
        }

        private ITDSPlayer? GetNextSpectatePlayerInSameTeam(ITDSPlayer start)
        {
            var teamlist = start.Team?.SpectateablePlayers;
            if (teamlist is null || teamlist.Count == 0)
                return null;
            int startindex = teamlist.IndexOf(start) + 1;
            if (startindex >= teamlist.Count - 1)
                startindex = 0;
            return teamlist[startindex];
        }

        private async Task<ITDSPlayer?> GetPreviousSpectatePlayerInAllTeams(ITDSPlayer start)
        {
            var teamIndex = (int?)start.Team?.Entity.Index ?? 0;
            var teamlist = await Lobby.Teams.Do(teams =>
            {
                if (teamIndex == 0)
                    teamIndex = teams.Length - 1;
                return teams[teamIndex].SpectateablePlayers;
            });

            if (teamlist is null)
                return null;
            var charIndex = teamlist.IndexOf(start) - 1;
            if (teamlist.Count == 0 || charIndex < 0)
            {
                var team = await Lobby.Teams.GetPreviousNonSpectatorTeamWithPlayers(start.Team);
                if (team is null)
                    return null;
                teamlist = team.SpectateablePlayers;
                if (teamlist is null)
                    return null;
                charIndex = teamlist.Count - 1;
            }
            return teamlist[charIndex];
        }

        private ITDSPlayer? GetPreviousSpectatePlayerInSameTeam(ITDSPlayer start)
        {
            var teamlist = start.Team?.SpectateablePlayers;
            if (teamlist is null || teamlist.Count == 0)
                return null;
            var startindex = teamlist.IndexOf(start) - 1;
            if (startindex < 0)
                startindex = teamlist.Count - 1;
            return teamlist[startindex];
        }
    }
}
