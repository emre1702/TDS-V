using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Players
{
    internal class CharCreateLobbyPlayers : BaseLobbyPlayers
    {
        public CharCreateLobbyPlayers(ICharCreateLobby lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex = 0)
        {
            if (player.Entity?.CharDatas is null)
                return false;
            var worked = await base.AddPlayer(player, teamIndex).ConfigureAwait(false);
            if (!worked)
                return false;

            var charDatasJson = Serializer.ToClient(player.Entity.CharDatas.SyncedData);

            NAPI.Task.RunSafe(() =>
            {
                player.Spawn(Lobby.MapHandler.SpawnPoint, Lobby.MapHandler.SpawnRotation);
                player.SetInvincible(true);
                player.Freeze(true);
                player.SetInvisible(true);

                player.TriggerEvent(ToClientEvent.StartCharCreator, charDatasJson, Lobby.MapHandler.Dimension);
            });

            return true;
        }
    }
}
