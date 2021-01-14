using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.LobbySystem.BansHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Chat;
using TDS.Server.Data.Interfaces.LobbySystem.Colshapes;
using TDS.Server.Data.Interfaces.LobbySystem.Database;
using TDS.Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Natives;
using TDS.Server.Data.Interfaces.LobbySystem.Notifications;
using TDS.Server.Data.Interfaces.LobbySystem.Players;
using TDS.Server.Data.Interfaces.LobbySystem.Sounds;
using TDS.Server.Data.Interfaces.LobbySystem.Sync;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Shared.Data.Enums;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
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