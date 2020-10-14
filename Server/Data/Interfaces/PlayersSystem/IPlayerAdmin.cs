using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Models;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerAdmin
    {
        AdminLevelDto Level { get; }
        string LevelName { get; }

        void Init(ITDSPlayer player);
    }
}
