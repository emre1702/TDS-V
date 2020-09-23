using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Shared.Core;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Players
{
    public class MapCreatorLobbyPlayers : BaseLobbyPlayers
    {
        public MapCreatorLobbyPlayers(IBaseLobby lobby, IBaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams, BaseLobbyBansHandler bans)
            : base(lobby, events, teams, bans)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            var worked = await base.AddPlayer(player, teamIndex);
            if (!worked)
                return false;

            NAPI.Task.Run(() =>
            {
                player.SetInvincible(true);
                player.Freeze(false);
            });
            return true;
        }

        public override async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            bool isLobbyOwner = IsLobbyOwner(player);
            var worked = await base.RemovePlayer(player);
            if (!worked)
                return false;

            if (isLobbyOwner && await Any())
                await SetNewRandomLobbyOwner();

            return true;
        }
    }
}
