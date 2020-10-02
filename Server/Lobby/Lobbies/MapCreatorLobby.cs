using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Chats;
using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class MapCreatorLobby : FreeroamLobby
    {
        public MapCreatorLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
        }

        public MapCreatorLobby(ITDSPlayer player, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler, LobbiesHandler lobbiesHandler)
            : base(CreateEntity(player, lobbiesHandler.MapCreateLobbyDummy.Entity), databaseHandler, langHelper, eventsHandler)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            void doForPlayersInMainThreadFuncProvider(Action<ITDSPlayer> action) => Players.DoInMain(action);
            lobbyDependencies ??= new BaseLobbyDependencies();

            lobbyDependencies.Events ??= new BaseLobbyEventsHandler(GlobalEventsHandler, this);
            lobbyDependencies.Database ??= new BaseLobbyDatabase(this, GlobalDatabaseHandler, lobbyDependencies.Events);
            lobbyDependencies.Teams ??= new BaseLobbyTeamsHandler(Entity);
            lobbyDependencies.Chat ??= new BaseLobbyChat(doForPlayersInMainThreadFuncProvider, LangHelper);
            lobbyDependencies.Bans ??= new MapCreatorLobbyBansHandler(lobbyDependencies.Database, lobbyDependencies.Events, LangHelper, lobbyDependencies.Chat, Entity);
            lobbyDependencies.Players ??= new BaseLobbyPlayers(Entity, lobbyDependencies.Events, lobbyDependencies.Teams, lobbyDependencies.Bans);

            base.InitDependencies(lobbyDependencies);
        }

        private static LobbyDb CreateEntity(ITDSPlayer player, LobbyDb dummy)
        {
            var entity = new Lobbies
            {
                Name = "MapCreator-" + player.Name ?? "?",
                Teams = new List<Teams> { new Teams { Index = 0, Name = player.Name ?? "?", ColorR = 222, ColorB = 222, ColorG = 222 } },
                Type = LobbyType.MapCreateLobby,
                OwnerId = player.Entity?.Id ?? -1,
                IsTemporary = true,
                DefaultSpawnX = dummy.DefaultSpawnX,
                DefaultSpawnY = dummy.DefaultSpawnY,
                DefaultSpawnZ = dummy.DefaultSpawnZ,
                DefaultSpawnRotation = dummy.DefaultSpawnRotation
            };

            return entity;
        }
    }
}
