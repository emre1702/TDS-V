using TDS.Server.Data.Interfaces.GamemodesSystem.Rounds;
using TDS.Server.GamemodesSystem.Weapons;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes
{
    public interface IArmsRaceGamemode : IBaseGamemode
    {
        new IArmsRaceGamemodeRounds Rounds { get; }
        new IArmsRaceGamemodeWeapons Weapons { get; }
    }
}
