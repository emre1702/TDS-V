﻿using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class MainMenuDeathmatch : BaseLobbyDeathmatch
    {
        public MainMenuDeathmatch(MainMenu lobby, IBaseLobbyEventsHandler events) : base(lobby, events)
        {
        }

        public override async Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            await base.OnPlayerDeath(player, killer, weapon);

            NAPI.Task.Run(() =>
            {
                player.Spawn(Lobby.MapHandler.SpawnPoint, Lobby.MapHandler.SpawnRotation);
                player.Freeze(true);
            });
        }
    }
}
