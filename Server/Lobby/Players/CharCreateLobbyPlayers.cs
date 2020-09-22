using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Players
{
    internal class CharCreateLobbyPlayers : BaseLobbyPlayers
    {
        public CharCreateLobbyPlayers(CharCreateLobby lobby, IBaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams, BaseLobbyBansHandler bans)
            : base(lobby, events, teams, bans)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex = 0)
        {
            if (player.Entity?.CharDatas is null)
                return false;
            var worked = await base.AddPlayer(player, teamIndex);
            if (!worked)
                return false;

            var charDatasJson = Serializer.ToClient(player.Entity.CharDatas.SyncedData);

            NAPI.Task.Run(() =>
            {
                player.SetInvincible(true);
                player.Freeze(true);

                player.TriggerEvent(ToClientEvent.StartCharCreator, charDatasJson, Lobby.MapHandler.Dimension);
            });

            return true;
        }
    }
}
