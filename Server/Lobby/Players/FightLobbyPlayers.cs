using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Players
{
    internal class FightLobbyPlayers : BaseLobbyPlayers
    {
        public FightLobbyPlayers(LobbyDb entity, BaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams) : base(entity, events, teams)
        {
        }

        public override async Task RemovePlayer(ITDSPlayer player)
        {
            await base.RemovePlayer(player);

            NAPI.Task.RunCustom(() =>
            {
                player.RemoveAllWeapons();
            });
        }
    }
}
