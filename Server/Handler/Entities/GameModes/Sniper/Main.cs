using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class Sniper : Gamemode
    {
        #region Public Constructors

        public Sniper(Arena lobby, MapDto map, IModAPI modAPI, Serializer serializer, ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(lobby, map, modAPI, serializer, settingsHandler, langHelper, invitationsHandler) { }

        #endregion Public Constructors

        #region Public Methods

        public static new void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Where(w => w.Type == WeaponType.SniperRifle)
                .Select(w => w.Hash)
                .ToHashSet();
        }

        #endregion Public Methods
    }
}
