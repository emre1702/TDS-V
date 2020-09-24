using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Data.Enums;

namespace TDS_Server.LobbySystem.Gamemodes
{
    public partial class Sniper : Gamemode, ISniper
    {
        public Sniper(ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(settingsHandler, langHelper, invitationsHandler) { }

        public static new void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Where(w => w.Type == WeaponType.SniperRifle)
                .Select(w => w.Hash)
                .ToHashSet();
        }
    }
}
