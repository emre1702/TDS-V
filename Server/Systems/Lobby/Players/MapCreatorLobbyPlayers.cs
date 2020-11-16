using System.Threading.Tasks;
using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.LobbySystem.Players
{
    public class MapCreatorLobbyPlayers : BaseLobbyPlayers
    {
        public MapCreatorLobbyPlayers(IBaseLobby lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            var worked = await base.AddPlayer(player, teamIndex).ConfigureAwait(false);
            if (!worked)
                return false;

            NAPI.Task.RunSafe(() =>
            {
                player.SetInvincible(true);
                player.SetInvisible(false);
                player.Freeze(false);
            });
            return true;
        }

        public override async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            bool isLobbyOwner = IsLobbyOwner(player);
            var worked = await base.RemovePlayer(player).ConfigureAwait(false);
            if (!worked)
                return false;

            if (isLobbyOwner && await Any().ConfigureAwait(false))
                await SetNewRandomLobbyOwner().ConfigureAwait(false);

            return true;
        }
    }
}
