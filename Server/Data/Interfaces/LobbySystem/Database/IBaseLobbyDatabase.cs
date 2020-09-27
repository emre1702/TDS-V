using System.Threading.Tasks;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Data.Interfaces.LobbySystem.Database
{
    public interface IBaseLobbyDatabase
    {
        Task AddBanEntity(PlayerBans ban);

        Task<PlayerBans> GetBan(int? playerId);

        Task<string> GetLastUsedSerial(int playerId);

        Task Remove<T>(object obj);

        Task Save();
    }
}
