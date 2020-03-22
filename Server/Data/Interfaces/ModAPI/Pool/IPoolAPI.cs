using System.Collections.Generic;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.Data.Interfaces.ModAPI.Pool
{
    public interface IPoolAPI
    {
        List<IPlayer> GetAllModPlayers();
        void RemoveAll();
    }
}
