using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Entities.Utility;
using TDS_Server.Handler.Helper;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Entities.GameModes
{
    partial class Gangwar : GameMode
    {
        private readonly GangwarArea? _gangwarArea;

        public Gangwar(Arena lobby, MapDto map, IModAPI modAPI, Serializer serializer, SettingsHandler settingsHandler, 
            GangwarAreasHandler gangwarAreasHandler, GangsHandler gangsHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler, LangHelper langHelper, InvitationsHandler invitationsHandler) 
            : base(lobby, map, modAPI, serializer, settingsHandler, langHelper, invitationsHandler)
        {
            var gangwarArea = gangwarAreasHandler.GetById(map.BrowserSyncedData.Id);
            if (gangwarArea is null)
            {
                /*lobby.SetRoundStatus(Enums.RoundStatus.RoundEnd, Enums.RoundEndReason.Error);
                return;*/
                // Create dummy gangwar area
                gangwarArea = new GangwarArea(map, settingsHandler, gangsHandler, dbContext, loggingHandler);
            }
            else if (!lobby.IsGangActionLobby)
            {
                gangwarArea = new GangwarArea(gangwarArea, settingsHandler, gangsHandler, dbContext, loggingHandler);
            }
            _gangwarArea = gangwarArea;
        }

        public static void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();
        }
    }
}
