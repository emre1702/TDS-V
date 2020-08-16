using BonusBotConnector.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Entity.Gamemodes.Gangwar;
using TDS_Server.Handler;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Entity.GangSystem.GangGamemodes.GangwarSystem
{
    internal partial class GangGangwar : Gangwar, IGangGamemode
    {
        #region Private Fields

        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly IGangwarArea _gangwarArea;

        #endregion Private Fields

        #region Public Constructors

        public GangGangwar(IArena lobby, MapDto map, Serializer serializer, ISettingsHandler settingsHandler, IServiceProvider serviceProvider,
            GangwarAreasHandler gangwarAreasHandler, GangsHandler gangsHandler, ILoggingHandler loggingHandler, LangHelper langHelper, InvitationsHandler invitationsHandler,
            BonusBotConnectorClient bonusBotConnectorClient, TDSBlipHandler tdsBlipHandler, TDSObjectHandler tdsObjectHandler, TDSTextLabelHandler tdsTextLabelHandler)
            : base(lobby, map, serializer, settingsHandler, langHelper, invitationsHandler, tdsBlipHandler, tdsObjectHandler, tdsTextLabelHandler)
        {
            _bonusBotConnectorClient = bonusBotConnectorClient;

            var gangwarArea = gangwarAreasHandler.GetById(map.BrowserSyncedData.Id);
            if (gangwarArea is null)
                gangwarArea = new GangwarArea(map, settingsHandler, gangsHandler, serviceProvider.GetRequiredService<TDSDbContext>(), loggingHandler,
                    serviceProvider, tdsBlipHandler);
            _gangwarArea = gangwarArea;
        }

        #endregion Public Constructors

        #region Public Properties

        public string AreaName => _gangwarArea.Entity!.Map.Name;

        public GangActionType Type => GangActionType.Gangwar;

        #endregion Public Properties
    }
}
