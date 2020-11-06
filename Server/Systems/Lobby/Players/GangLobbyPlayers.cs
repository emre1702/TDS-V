﻿using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Handler.Extensions;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.Players
{
    public class GangLobbyPlayers : BaseLobbyPlayers
    {
        public GangLobbyPlayers(GangLobby lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex = 0)
        {
            var team = player.Gang.TeamHandler.GangLobbyTeam;
            var worked = await base.AddPlayer(player, team.Entity.Index).ConfigureAwait(false);
            if (!worked)
                return false;

            var spawnPoint = player.Gang.HouseHandler.House?.Position ?? Lobby.MapHandler.SpawnPoint;
            var spawnRotation = player.Gang.HouseHandler.House?.SpawnRotation ?? Lobby.MapHandler.SpawnRotation;

            NAPI.Task.RunSafe(() =>
            {
                player.Spawn(spawnPoint, spawnRotation);
                player.Freeze(false);
                player.SetInvincible(true);
                player.SetInvisible(false);
            });

            return true;
        }
    }
}