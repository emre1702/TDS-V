using System.Threading.Tasks;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Data.Interfaces.LobbySystem.Database
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
