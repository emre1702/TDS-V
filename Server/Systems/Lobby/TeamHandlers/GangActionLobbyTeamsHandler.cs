using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler.Helper;
using TDS.Server.LobbySystem.Lobbies;

namespace TDS.Server.LobbySystem.TeamHandlers
{
    public class GangActionLobbyTeamsHandler : RoundFightLobbyTeamsHandler, IGangActionLobbyTeamsHandler
    {
        public ITeam Attacker { get; }
        public ITeam Owner { get; }

        public int AttackerAmount
        {
            get
            {
                lock (_attackerPlayerIds)
                {
                    return Math.Max(_attackerPlayerIds.Count, _settingsHandler.ServerSettings.MinPlayersOnlineForGangwar);
                }
            }
        }

        public int OwnerAmount
        {
            get
            {
                lock (_ownerPlayerIds)
                {
                    return Math.Max(_ownerPlayerIds.Count, _settingsHandler.ServerSettings.MinPlayersOnlineForGangwar);
                }
            }
        }

        private readonly HashSet<int> _attackerPlayerIds = new HashSet<int>();
        private readonly HashSet<int> _ownerPlayerIds = new HashSet<int>();

        protected new IGangActionLobby Lobby => (IGangActionLobby)base.Lobby;
        private readonly ISettingsHandler _settingsHandler;

        public GangActionLobbyTeamsHandler(GangActionLobby lobby, IRoundFightLobbyEventsHandler events, LangHelper langHelper, ISettingsHandler settingsHandler,
            ITeamsProvider teamsProvider)
            : base(lobby, events, langHelper, teamsProvider)
        {
            _settingsHandler = settingsHandler;

            events.PlayerLeftAfter += Events_PlayerLeftAfter;

            var teams = GetTeams();
            Attacker = teams[(int)GangActionLobbyTeamIndex.Attacker];
            Owner = teams[(int)GangActionLobbyTeamIndex.Owner];
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);
            if (Events.PlayerLeftAfter is { })
                Events.PlayerLeftAfter -= Events_PlayerLeftAfter;
        }

        protected override async ValueTask OnPlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            await base.OnPlayerJoined(data).ConfigureAwait(false);

            if (data.TeamIndex == (int)GangActionLobbyTeamIndex.Attacker)
                lock (_attackerPlayerIds) { _attackerPlayerIds.Add(data.Player.Id); }
            else if (data.TeamIndex == (int)GangActionLobbyTeamIndex.Owner)
                lock (_ownerPlayerIds) { _ownerPlayerIds.Add(data.Player.Id); }
        }

        private ValueTask Events_PlayerLeftAfter((ITDSPlayer Player, int TeamIndex) data)
        {
            if (!Lobby.Deathmatch.Damage.DamageDealtThisRound)
            {
                if (data.TeamIndex == (int)GangActionLobbyTeamIndex.Attacker)
                    lock (_attackerPlayerIds) { _attackerPlayerIds.Remove(data.Player.Id); }
                else if (data.TeamIndex == (int)GangActionLobbyTeamIndex.Owner)
                    lock (_ownerPlayerIds) { _ownerPlayerIds.Remove(data.Player.Id); }
            }
            return default;
        }

        public bool HasTeamFreePlace(bool isAttacker)
        {
            lock (_attackerPlayerIds)
            {
                lock (_ownerPlayerIds)
                {
                    if (isAttacker)
                    {
                        return _attackerPlayerIds.Count < _settingsHandler.ServerSettings.AmountPlayersAllowedInGangwarTeamBeforeCountCheck
                            || _attackerPlayerIds.Count < _ownerPlayerIds.Count + (_settingsHandler.ServerSettings.GangwarAttackerCanBeMore ? 1 : 0);
                    }
                    else
                    {
                        return _ownerPlayerIds.Count < _settingsHandler.ServerSettings.AmountPlayersAllowedInGangwarTeamBeforeCountCheck
                            || _ownerPlayerIds.Count < _attackerPlayerIds.Count + (_settingsHandler.ServerSettings.GangwarOwnerCanBeMore ? 1 : 0);
                    }
                }
            }
        }

        public bool HasBeenInLobby(ITDSPlayer player, int teamIndex)
        {
            if (teamIndex == (int)GangActionLobbyTeamIndex.Attacker)
            {
                lock (_attackerPlayerIds) { return _attackerPlayerIds.Contains(player.Id); };
            }
            else if (teamIndex == (int)GangActionLobbyTeamIndex.Owner)
            {
                lock (_ownerPlayerIds) { return _ownerPlayerIds.Contains(player.Id); };
            }
            return false;
        }
    }
}
