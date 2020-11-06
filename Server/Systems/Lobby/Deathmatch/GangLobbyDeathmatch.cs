﻿using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class GangLobbyDeathmatch : BaseLobbyDeathmatch
    {
        public GangLobbyDeathmatch(IBaseLobby lobby, IBaseLobbyEventsHandler events) : base(lobby, events)
        {
        }

        public override async Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            await base.OnPlayerDeath(player, killer, weapon).ConfigureAwait(false);

            NAPI.Task.RunSafe(() =>
                player.Spawn(player.Position, player.Rotation.Z));
        }
    }
}