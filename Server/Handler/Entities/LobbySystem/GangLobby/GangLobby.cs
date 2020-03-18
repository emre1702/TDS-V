using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class GangLobby : FightLobby
    {
        private readonly GangwarAreasHandler _gangwarAreasHandler;
        private readonly GangsHandler _gangsHandler;
        private readonly IServiceProvider _serviceProvider;

        public GangLobby(Lobbies Entity, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, IModAPI modAPI, LobbiesHandler lobbiesHandler, 
            SettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, GangsHandler gangsHandler, EventsHandler eventsHandler, 
            GangwarAreasHandler gangwarAreasHandler, IServiceProvider serviceProvider) 
            : base(Entity, false, dbContext, loggingHandler, serializer, modAPI, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler, eventsHandler)
        {
            _gangwarAreasHandler = gangwarAreasHandler;
            _gangsHandler = gangsHandler;
            _serviceProvider = serviceProvider;

            foreach (var team in Teams)
            {
                var teamId = team.Entity.Id;
                var gang = gangsHandler.GetByTeamId(teamId);
                if (gang != null)
                {
                    gang.GangLobbyTeam = team;
                }
            }
        }

        public IEnumerable<GangLobby> GetAllDerivedLobbies()
        {
            return LobbiesHandler.Lobbies.Where(l => l is GangLobby && l.Type != LobbyType.GangLobby).Cast<GangLobby>();
        }
    }
}
