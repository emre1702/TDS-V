using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.RAGE.Startup
{
    class BaseAPI : IModAPI
    {
        public IPlayerAPI Player { get; private set; }

        public ISyncAPI Sync { get; private set; }

        internal BaseAPI()
        {
            Player = new PlayerAPI();
            Sync = new SyncAPI();
        }
    }
}
