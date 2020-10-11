using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
#nullable enable

    public interface IPlayerDatabaseHandler
    {
        IDatabaseHandler Database { get; }
        Players? Entity { get; set; }

        void CheckSaveData();

        void Init(ITDSPlayer player);

        ValueTask SaveData(bool force = false);
    }
}
