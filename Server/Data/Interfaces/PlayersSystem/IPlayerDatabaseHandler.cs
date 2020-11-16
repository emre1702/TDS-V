using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Data.Interfaces.PlayersSystem
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
