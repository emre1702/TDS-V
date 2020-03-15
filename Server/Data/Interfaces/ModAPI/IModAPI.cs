using TDS_Server.Data.Interfaces.ModAPI.Chat;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Sync;
using TDS_Server.Data.Interfaces.ModAPI.Thread;

namespace TDS_Server.Data.Interfaces.ModAPI
{
    public interface IModAPI
    {
        IChatAPI Chat { get; }
        IPlayerAPI Player { get; }
        ISyncAPI Sync { get; }
        IThreadAPI Thread { get; }
    }
}
