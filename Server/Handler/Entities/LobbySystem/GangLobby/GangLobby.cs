﻿using BonusBotConnector.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class GangLobby : Lobby
    {
        #region Private Fields

        private readonly GangHousesHandler _gangHousesHandler;
        private readonly GangLevelsHandler _gangLevelsHandler;
        private readonly GangsHandler _gangsHandler;
        private readonly GangwarAreasHandler _gangwarAreasHandler;
        private readonly IServiceProvider _serviceProvider;

        #endregion Private Fields

        #region Public Constructors

        public GangLobby(Lobbies Entity, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, IModAPI modAPI, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, GangsHandler gangsHandler, EventsHandler eventsHandler,
            GangwarAreasHandler gangwarAreasHandler, IServiceProvider serviceProvider, GangLevelsHandler gangLevelsHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler, GangHousesHandler gangHousesHandler)
            : base(Entity, false, dbContext, loggingHandler, serializer, modAPI, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler, eventsHandler,
                  bonusBotConnectorClient, bansHandler)
        {
            _gangwarAreasHandler = gangwarAreasHandler;
            _gangsHandler = gangsHandler;
            _serviceProvider = serviceProvider;
            _gangHousesHandler = gangHousesHandler;
            _gangLevelsHandler = gangLevelsHandler;

            LoadTeams();
            LoadHouses();
        }

        #endregion Public Constructors

        #region Public Methods

        public IEnumerable<GangLobby> GetAllDerivedLobbies()
        {
            //Todo: Use GangActionLobby instead of GangLobby
            return LobbiesHandler.Lobbies.Where(l => l is GangLobby && l.Type != LobbyType.GangLobby).Cast<GangLobby>();
        }

        #endregion Public Methods
    }
}
