using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerAdmin
    {
        AdminLevelDto Level { get; }
        string LevelName { get; }

        void Init(ITDSPlayer player);
    }
}
