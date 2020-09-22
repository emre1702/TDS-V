using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Shared.Data.Enums;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class CharCreateLobby : BaseLobby
    {
        public CharCreateLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
        }

        public CharCreateLobby(ITDSPlayer player, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler, LobbiesHandler lobbiesHandler)
            : base(CreateEntity(player, lobbiesHandler.CharCreateLobbyDummy.Entity), databaseHandler, langHelper, eventsHandler)
        {
        }

        private static LobbyDb CreateEntity(ITDSPlayer player, LobbyDb dummy)
        {
            var entity = new LobbyDb
            {
                Name = "CharCreator-" + player.Name,
                Teams = new List<Teams> { new Teams { Index = 0, Name = player.Name, ColorR = 222, ColorB = 222, ColorG = 222 } },
                Type = LobbyType.CharCreateLobby,
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
