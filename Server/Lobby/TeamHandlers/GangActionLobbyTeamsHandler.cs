using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
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

        public GangActionLobbyTeamsHandler(GangActionLobby lobby, IRoundFightLobbyEventsHandler events, LangHelper langHelper, ISettingsHandler settingsHandler)
            : base(lobby, events, langHelper)
        {
            _settingsHandler = settingsHandler;

            events.PlayerLeftAfter += Events_PlayerLeftAfter;

            var teams = GetTeams();
            Attacker = teams[(int)GangActionLobbyTeamIndex.Attacker];
            Owner = teams[(int)GangActionLobbyTeamIndex.Owner];
        }

        protected override async ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            await base.Events_PlayerJoined(data);

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
