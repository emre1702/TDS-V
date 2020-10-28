﻿using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.LobbySystem.Players
{
    public class GangActionLobbyPlayers : RoundFightLobbyPlayers
    {
        protected new IGangActionLobby Lobby => (IGangActionLobby)base.Lobby;

        public GangActionLobbyPlayers(IGangActionLobby lobby, IRoundFightLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        public override Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            if (Lobby.Teams.HasBeenInLobby(player, teamIndex))
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.GANGACTION_CANT_JOIN_AGAIN));
                return Task.FromResult(false);
            }

            if (!Lobby.Teams.HasTeamFreePlace(teamIndex == (int)GangActionLobbyTeamIndex.Attacker))
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.TEAM_ALREADY_FULL_INFO));
                return Task.FromResult(false);
            }

            return base.AddPlayer(player, teamIndex);
        }
    }
}
