using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Entities.Utility;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.GameModes
{
    partial class Gangwar : GameMode
    {
        #region Private Fields

        private readonly GangwarArea? _gangwarArea;

        #endregion Private Fields

        #region Public Constructors

        public Gangwar(Arena lobby, MapDto map, IModAPI modAPI, Serializer serializer, ISettingsHandler settingsHandler, IServiceProvider serviceProvider,
            GangwarAreasHandler gangwarAreasHandler, GangsHandler gangsHandler, ILoggingHandler loggingHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(lobby, map, modAPI, serializer, settingsHandler, langHelper, invitationsHandler)
        {
            var gangwarArea = gangwarAreasHandler.GetById(map.BrowserSyncedData.Id);
            if (gangwarArea is null)
            {
                /*lobby.SetRoundStatus(Enums.RoundStatus.RoundEnd, Enums.RoundEndReason.Error);
                return;*/
                // Create dummy gangwar area
                gangwarArea = new GangwarArea(map, modAPI, settingsHandler, gangsHandler, serviceProvider.GetRequiredService<TDSDbContext>(), loggingHandler);
            }
            else if (!lobby.IsGangActionLobby)
            {
                gangwarArea = new GangwarArea(gangwarArea, modAPI, settingsHandler, gangsHandler, serviceProvider.GetRequiredService<TDSDbContext>(), loggingHandler);
            }
            _gangwarArea = gangwarArea;
        }

        #endregion Public Constructors
    }
}
