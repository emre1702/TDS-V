using BonusBotConnector.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Core;
using TDS_Server.Handler.Account;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class GangLobby : Lobby
    {
        private readonly GangwarAreasHandler _gangwarAreasHandler;
        private readonly GangsHandler _gangsHandler;
        private readonly IServiceProvider _serviceProvider;

        public GangLobby(Lobbies Entity, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, IModAPI modAPI, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, GangsHandler gangsHandler, EventsHandler eventsHandler,
            GangwarAreasHandler gangwarAreasHandler, IServiceProvider serviceProvider, WeaponDatasLoadingHandler weaponDatasLoadingHandler, 
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler)
            : base(Entity, false, dbContext, loggingHandler, serializer, modAPI, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler, eventsHandler,
                  bonusBotConnectorClient, bansHandler)
        {
            _gangwarAreasHandler = gangwarAreasHandler;
            _gangsHandler = gangsHandler;
            _serviceProvider = serviceProvider;

            /*LoadTeams();
            LoadGangLevels();
            LoadHouses();*/
        }

        public IEnumerable<GangLobby> GetAllDerivedLobbies()
        {
            //Todo: Use GangActionLobby instead of GangLobby
            return LobbiesHandler.Lobbies.Where(l => l is GangLobby && l.Type != LobbyType.GangLobby).Cast<GangLobby>();
        }
    }
}
