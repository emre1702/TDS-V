using BonusBotConnector.Client;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class CharCreateLobby : Lobby
    {
        public CharCreateLobby(ITDSPlayer player, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer,
            LobbiesHandler lobbiesHandler, ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler)

            : this(CreateEntity(player, lobbiesHandler.CharCreateLobbyDummy.Entity), dbContext, loggingHandler, serializer, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler,
                  eventsHandler, bonusBotConnectorClient, bansHandler)
        {
        }

        public CharCreateLobby(Lobbies entity, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer,
            LobbiesHandler lobbiesHandler, ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler)

            : base(entity, false, dbContext, loggingHandler, serializer, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler,
                  eventsHandler, bonusBotConnectorClient, bansHandler)
        {
        }

        private static Lobbies CreateEntity(ITDSPlayer player, Lobbies dummy)
        {
            var entity = new Lobbies
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
