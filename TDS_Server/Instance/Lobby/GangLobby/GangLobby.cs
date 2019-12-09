﻿using System.Collections.Generic;
using System.Linq;
using TDS_Common.Enum;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Lobby;

namespace TDS_Server.Instance.Lobby
{
    partial class GangLobby : FightLobby
    {
        public GangLobby(Lobbies lobbyEntity) : base(lobbyEntity)
        {
            foreach (var team in Teams)
            {
                var teamId = team.Entity.Id;
                var gang = Gang.GetByTeamId(teamId);
                if (gang != null)
                {
                    gang.GangLobbyTeam = team;
                }
            }
        }

        public IEnumerable<GangLobby> GetAllDerivedLobbies()
        {
            return LobbyManager.Lobbies.Where(l => l is GangLobby && l.Type != ELobbyType.GangLobby).Cast<GangLobby>();
        }
    }
}