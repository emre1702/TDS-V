using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Player;

namespace TDS_Client.Data.Interfaces.ModAPI.Sync
{
    public interface IPoolPlayersAPI : IPoolEntityAPI<IPlayer>
    {
        IEnumerable<IPlayer> All { get; }
    }
}
