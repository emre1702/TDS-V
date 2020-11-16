using System;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.GamemodesSystem.Rounds;
using TDS.Server.GamemodesSystem.DependenciesModels;
using TDS.Server.GamemodesSystem.Rounds;
using TDS.Server.GamemodesSystem.Weapons;

namespace TDS.Server.GamemodesSystem.Gamemodes
{
    public class ArmsRaceGamemode : BaseGamemode, IArmsRaceGamemode
    {
        public new IArmsRaceGamemodeRounds Rounds => (IArmsRaceGamemodeRounds)base.Rounds;
        public new IArmsRaceGamemodeWeapons Weapons => (IArmsRaceGamemodeWeapons)base.Weapons;

        public ArmsRaceGamemode(ISettingsHandler settingsHandler, IServiceProvider serviceProvider) : base(settingsHandler, serviceProvider)
        {
        }

        protected override void InitDependencies(BaseGamemodeDependencies? d = null)
        {
            d ??= new BaseGamemodeDependencies();

            d.Rounds ??= new ArmsRaceRounds(Lobby, this);
            d.Weapons ??= new ArmsRaceWeapons(Lobby, this);

            base.InitDependencies(d);
        }
    }
}
