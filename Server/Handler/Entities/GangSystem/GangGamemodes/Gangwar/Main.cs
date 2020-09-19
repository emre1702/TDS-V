using System;
using BonusBotConnector.Client;
using Microsoft.Extensions.DependencyInjection;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities.Gamemodes;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Entities.Utility;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.GangSystem.GangGamemodes
{
    internal partial class GangGangwar : Gangwar, IGangGamemode
    {
        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly GangwarArea _gangwarArea;

        public GangGangwar(Arena lobby, MapDto map, ISettingsHandler settingsHandler, IServiceProvider serviceProvider,
            GangwarAreasHandler gangwarAreasHandler, GangsHandler gangsHandler, ILoggingHandler loggingHandler, LangHelper langHelper, InvitationsHandler invitationsHandler,
            BonusBotConnectorClient bonusBotConnectorClient)
            : base(lobby, map, settingsHandler, langHelper, invitationsHandler)
        {
            _bonusBotConnectorClient = bonusBotConnectorClient;

            var gangwarArea = gangwarAreasHandler.GetById(map.BrowserSyncedData.Id);
            if (gangwarArea is null)
                gangwarArea = new GangwarArea(map, settingsHandler, gangsHandler, serviceProvider.GetRequiredService<TDSDbContext>(), loggingHandler);
            _gangwarArea = gangwarArea;
        }

        public string AreaName => _gangwarArea.Entity!.Map.Name;

        public GangActionType Type => GangActionType.Gangwar;
    }
}