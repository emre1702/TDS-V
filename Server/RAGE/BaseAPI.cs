using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Chat;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Sync;
using TDS_Server.Data.Interfaces.ModAPI.Thread;
using TDS_Server.RAGE.Chat;
using TDS_Server.RAGE.Player;
using TDS_Server.RAGE.Thread;

namespace TDS_Server.RAGE.Startup
{
    class BaseAPI : IModAPI
    {
        public IChatAPI Chat { get; }
        public IPlayerAPI Player { get; }
        public ISyncAPI Sync { get; }
        public IThreadAPI Thread { get; }

        internal BaseAPI()
        {
            Chat = new ChatAPI();
            Player = new PlayerAPI();
            Sync = new SyncAPI();
            Thread = new ThreadAPI();
        }
    }
}
