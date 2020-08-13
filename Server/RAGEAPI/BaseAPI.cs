
using TDS_Server.RAGEAPI.Blip;
using TDS_Server.RAGEAPI.Chat;
using TDS_Server.RAGEAPI.ClientEvent;
using TDS_Server.RAGEAPI.ColShape;
using TDS_Server.RAGEAPI.MapObject;
using TDS_Server.RAGEAPI.Marker;
using TDS_Server.RAGEAPI.Native;
using TDS_Server.RAGEAPI.Player;
using TDS_Server.RAGEAPI.Pool;
using TDS_Server.RAGEAPI.Server;
using TDS_Server.RAGEAPI.Sync;
using TDS_Server.RAGEAPI.TextLabel;
using TDS_Server.RAGEAPI.Thread;
using TDS_Server.RAGEAPI.Vehicle;

namespace TDS_Server.RAGEAPI
{
    internal class BaseAPI : IModAPI
    {
        #region Internal Constructors

        internal BaseAPI()
        {
            Blip = new BlipAPI();
            Chat = new ChatAPI();
            ClientEvent = new ClientEventAPI();
            ColShape = new ColShapeAPI();
            MapObject = new MapObjectAPI();
            Marker = new MarkerAPI();
            Native = new NativeAPI();
            Player = new PlayerAPI();
            Pool = new PoolAPI();
            Resource = new ResourceAPI();
            Server = new ServerAPI();
            Sync = new SyncAPI();
            TextLabel = new TextLabelAPI();
            Thread = new ThreadAPI();
            Vehicle = new VehicleAPI();
        }

        #endregion Internal Constructors

        #region Public Properties

        public IBlipAPI Blip { get; }
        public IChatAPI Chat { get; }
        public IClientEventAPI ClientEvent { get; }
        public ITDSColShapeAPI ColShape { get; }
        public IMapObjectAPI MapObject { get; }
        public IMarkerAPI Marker { get; }
        public INativeAPI Native { get; }
        public IPlayerAPI Player { get; }
        public IPoolAPI Pool { get; }
        public IResourceAPI Resource { get; }
        public IServerAPI Server { get; }
        public ISyncAPI Sync { get; }
        public ITextLabelAPI TextLabel { get; }
        public IThreadAPI Thread { get; }
        public IVehicleAPI Vehicle { get; }

        #endregion Public Properties

        #region Public Methods

        public bool CheckHasErrors(ILoggingHandler loggingHandler)
        {
            var codeMistakesChecker = new CodeMistakesChecker(loggingHandler);
            return codeMistakesChecker.CheckHasErrors();
        }

        #endregion Public Methods
    }
}
