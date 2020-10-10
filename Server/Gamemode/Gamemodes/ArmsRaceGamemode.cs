using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.GamemodesSystem.Rounds;
using TDS_Server.GamemodesSystem.DependenciesModels;
using TDS_Server.GamemodesSystem.Rounds;
using TDS_Server.GamemodesSystem.Weapons;

namespace TDS_Server.GamemodesSystem.Gamemodes
{
    public class ArmsRaceGamemode : BaseGamemode, IArmsRaceGamemode
    {
        public new IArmsRaceGamemodeRounds Rounds => (IArmsRaceGamemodeRounds)base.Rounds;
        public new IArmsRaceGamemodeWeapons Weapons => (IArmsRaceGamemodeWeapons)base.Weapons;

        public ArmsRaceGamemode(ISettingsHandler settingsHandler) : base(settingsHandler)
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
