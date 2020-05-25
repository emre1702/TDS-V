using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.GameModes.Normal
{
    partial class Normal : GameMode
    {
        #region Public Constructors

        public Normal(Arena lobby, MapDto map, IModAPI modAPI, Serializer serializer, ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(lobby, map, modAPI, serializer, settingsHandler, langHelper, invitationsHandler)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public static void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();
        }

        #endregion Public Methods
    }
}
