using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Database.Entity;
using TDS_Server.GamemodesSystem.DependenciesModels;
using TDS_Server.GamemodesSystem.Weapons;

namespace TDS_Server.GamemodesSystem.Gamemodes
{
    public class SniperGamemode : BaseGamemode, ISniperGamemode
    {
        public SniperGamemode(ISettingsHandler settingsHandler) : base(settingsHandler)
        {
        }

        public static void Init(TDSDbContext dbContext)
        {
            SniperWeapons.LoadAllowedWeapons(dbContext);
        }

        protected override void InitDependencies(BaseGamemodeDependencies? d = null)
        {
            d ??= new BaseGamemodeDependencies();

            d.Weapons = new SniperWeapons();

            base.InitDependencies(d);
        }
    }
}
