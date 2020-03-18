using System.Linq;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Data.Models.Map;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Shared.Manager.Utility;
using TDS_Server.Handler.Helper;

namespace TDS_Server.Handler.Entities.GameModes.Normal
{
    partial class Normal : GameMode
    {
        public Normal(Arena lobby, MapDto map, IModAPI modAPI, Serializer serializer, SettingsHandler settingsHandler, LangHelper langHelper) 
            : base(lobby, map, modAPI, serializer, settingsHandler, langHelper) 
        { 
            
        }

        public static void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();
        }
    }
}
