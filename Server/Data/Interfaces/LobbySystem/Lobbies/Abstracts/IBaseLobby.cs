using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.BansHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Chat;
using TDS_Server.Data.Interfaces.LobbySystem.Colshapes;
using TDS_Server.Data.Interfaces.LobbySystem.Database;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Natives;
using TDS_Server.Data.Interfaces.LobbySystem.Notifications;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Data.Interfaces.LobbySystem.Sounds;
using TDS_Server.Data.Interfaces.LobbySystem.Sync;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Shared.Data.Enums;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
{
    public interface IBaseLobby
    {
        public LobbyType Type => Entity.Type;
        public bool IsOfficial => Entity.IsOfficial;
        public bool IsRemoved => Events.IsRemoved;

        LobbyDb Entity { get; }
        TaskCompletionSource<bool> IsCreatingTask { get; }

        IBaseLobbyBansHandler Bans { get; }
        IBaseLobbyChat Chat { get; }
        IBaseLobbyColshapesHandler ColshapesHandler { get; }
        IBaseLobbyDatabase Database { get; }
        IBaseLobbyDeathmatch Deathmatch { get; }
        IBaseLobbyEventsHandler Events { get; }
        ILoggingHandler LoggingHandler { get; }
        IBaseLobbyMapHandler MapHandler { get; }
        IBaseLobbyNatives Natives { get; }
        IBaseLobbyNotifications Notifications { get; }
        IBaseLobbyPlayers Players { get; }
        IBaseLobbySoundsHandler Sounds { get; }
        IBaseLobbySync Sync { get; }
        IBaseLobbyTeamsHandler Teams { get; }

        Task Remove();
    }
}
