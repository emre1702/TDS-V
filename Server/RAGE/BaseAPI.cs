using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Events;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.RAGE.Events.Custom;

namespace TDS_Server.RAGE.Startup
{
    class BaseAPI : IModAPI
    {
        internal static BaseAPI Instance { get; private set; }

        public IPlayerAPI Player { get; private set; }
        public IEvents Events { get; private set; }

        internal BaseAPI()
        {
            Instance = this;

            Player = new PlayerAPI();
            Events = new BaseCustomEvents();
        }
    }
}
