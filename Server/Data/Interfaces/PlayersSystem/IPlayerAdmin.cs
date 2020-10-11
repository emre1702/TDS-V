using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Models;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerAdmin
    {
        AdminLevelDto AdminLevel { get; }
        string AdminLevelName { get; }

        void Init(ITDSPlayer player);
    }
}
