using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Database.Entity;
using TDS.Server.GamemodesSystem.DependenciesModels;
using TDS.Server.GamemodesSystem.Weapons;

namespace TDS.Server.GamemodesSystem.Gamemodes
{
    public class SniperGamemode : BaseGamemode, ISniperGamemode
    {
        public SniperGamemode(ISettingsHandler settingsHandler, IServiceProvider serviceProvider) : base(settingsHandler, serviceProvider)
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
