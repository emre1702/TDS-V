using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.Chat;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Server.Data.Interfaces.ModAPI.Marker;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Pool;
using TDS_Server.Data.Interfaces.ModAPI.Server;
using TDS_Server.Data.Interfaces.ModAPI.Sync;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;
using TDS_Server.Data.Interfaces.ModAPI.Thread;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.RAGE.Blip;
using TDS_Server.RAGE.Chat;
using TDS_Server.RAGE.ColShape;
using TDS_Server.RAGE.MapObject;
using TDS_Server.RAGE.Marker;
using TDS_Server.RAGE.Player;
using TDS_Server.RAGE.Pool;
using TDS_Server.RAGE.Server;
using TDS_Server.RAGE.TextLabel;
using TDS_Server.RAGE.Thread;
using TDS_Server.RAGE.Vehicle;

namespace TDS_Server.RAGE.Startup
{
    class BaseAPI : IModAPI
    {
        public IBlipAPI Blip { get; }
        public IChatAPI Chat { get; }
        public IColShapeAPI ColShape { get; }
        public IMapObjectAPI MapObject { get; }
        public IMarkerAPI Marker { get; }
        public IPlayerAPI Player { get; }
        public IPoolAPI Pool { get; }
        public IResourceAPI Resource { get; }
        public IServerAPI Server { get; }
        public ISyncAPI Sync { get; }
        public ITextLabelAPI TextLabel { get; }
        public IThreadAPI Thread { get; }
        public IVehicleAPI Vehicle { get; }


        internal BaseAPI()
        {
            Blip = new BlipAPI();
            Chat = new ChatAPI();
            ColShape = new ColShapeAPI();
            MapObject = new MapObjectAPI();
            Marker = new MarkerAPI();
            Player = new PlayerAPI();
            Pool = new PoolAPI();
            Resource = new ResourceAPI();
            Server = new ServerAPI();
            Sync = new SyncAPI();
            TextLabel = new TextLabelAPI();
            Thread = new ThreadAPI();
            Vehicle = new VehicleAPI();
        }
    }
}
