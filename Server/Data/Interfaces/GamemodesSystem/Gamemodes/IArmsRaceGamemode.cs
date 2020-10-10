using TDS_Server.Data.Interfaces.GamemodesSystem.Rounds;
using TDS_Server.GamemodesSystem.Weapons;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes
{
    public interface IArmsRaceGamemode : IBaseGamemode
    {
        new IArmsRaceGamemodeRounds Rounds { get; }
        new IArmsRaceGamemodeWeapons Weapons { get; }
    }
}
