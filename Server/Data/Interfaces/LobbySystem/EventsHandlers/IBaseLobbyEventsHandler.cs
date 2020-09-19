using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Utility;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers
{
#nullable enable

    public interface IBaseLobbyEventsHandler
    {
        public delegate void LobbyCreatedAfterDelegate(LobbyDb entity);

        public delegate void LobbyDelegate(IBaseLobby lobby);

        public delegate void PlayerDelegate(ITDSPlayer player);

        bool IsRemoved { get; }
        AsyncTaskEvent<LobbyDb>? LobbyCreated { get; set; }
        AsyncTaskEvent<IBaseLobby>? LobbyRemove { get; set; }
        AsyncValueTaskEvent<ITDSPlayer>? PlayerLeftLobby { get; set; }

        event LobbyCreatedAfterDelegate? LobbyCreatedAfter;

        event LobbyDelegate? LobbyRemoveAfter;

        event PlayerDelegate? PlayerJoinedLobby;

        event PlayerDelegate? PlayerLeftLobbyAfter;

        Task TriggerLobbyCreated(LobbyDb entity);

        Task TriggerLobbyRemove(IBaseLobby lobby);

        void TriggerPlayerJoinedLobby(ITDSPlayer player);

        ValueTask TriggerPlayerLeftLobby(ITDSPlayer player);
    }
}
