using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Sync;

namespace TDS_Server.Data.Interfaces.ModAPI
{
    public interface IModAPI
    {
        IPlayerAPI Player { get; }
        ISyncAPI Sync { get; }
    }
}
