using TDS_Client.Data.Interfaces.RAGE.Game;
using TDS_Client.Data.Interfaces.RAGE.Game.Audio;
using TDS_Client.Data.Interfaces.RAGE.Game.Blip;
using TDS_Client.Data.Interfaces.RAGE.Game.Browser;
using TDS_Client.Data.Interfaces.RAGE.Game.Cam;
using TDS_Client.Data.Interfaces.RAGE.Game.Chat;
using TDS_Client.Data.Interfaces.RAGE.Game.Checkpoint;
using TDS_Client.Data.Interfaces.RAGE.Ui.Console;
using TDS_Client.Data.Interfaces.RAGE.Game.Control;
using TDS_Client.Data.Interfaces.RAGE.Game.Cursor;
using TDS_Client.Data.Interfaces.RAGE.Game.Cutscene;
using TDS_Client.Data.Interfaces.RAGE.Game.Discord;
using TDS_Client.Data.Interfaces.RAGE.Game.Entity;
using TDS_Client.Data.Interfaces.RAGE.Game.Event;
using TDS_Client.Data.Interfaces.RAGE.Game.Graphics;
using TDS_Client.Data.Interfaces.RAGE.Game.Input;
using TDS_Client.Data.Interfaces.RAGE.Game.Locale;
using TDS_Client.Data.Interfaces.RAGE.Game.MapObject;
using TDS_Client.Data.Interfaces.RAGE.Game.Misc;
using TDS_Client.Data.Interfaces.RAGE.Game.Nametags;
using TDS_Client.Data.Interfaces.RAGE.Game.Native;
using TDS_Client.Data.Interfaces.RAGE.Game.Ped;
using TDS_Client.Data.Interfaces.RAGE.Game.Player;
using TDS_Client.Data.Interfaces.RAGE.Game.Pool;
using TDS_Client.Data.Interfaces.RAGE.Game.Shapetest;
using TDS_Client.Data.Interfaces.RAGE.Game.Stats;
using TDS_Client.Data.Interfaces.RAGE.Game.Streaming;
using TDS_Client.Data.Interfaces.RAGE.Game.Sync;
using TDS_Client.Data.Interfaces.RAGE.Game.Ui;
using TDS_Client.Data.Interfaces.RAGE.Game.Utils;
using TDS_Client.Data.Interfaces.RAGE.Game.Vehicle;
using TDS_Client.Data.Interfaces.RAGE.Game.Voice;
using TDS_Client.Data.Interfaces.RAGE.Game.Weapon;
using TDS_Client.Data.Interfaces.RAGE.Game.Windows;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Audio;
using TDS_Client.RAGEAPI.Blip;
using TDS_Client.RAGEAPI.Browser;
using TDS_Client.RAGEAPI.Cam;
using TDS_Client.RAGEAPI.Chat;
using TDS_Client.RAGEAPI.Checkpoint;
using TDS_Client.RAGEAPI.Console;
using TDS_Client.RAGEAPI.Control;
using TDS_Client.RAGEAPI.Cursor;
using TDS_Client.RAGEAPI.Cutscene;
using TDS_Client.RAGEAPI.Discord;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Event;
using TDS_Client.RAGEAPI.Graphics;
using TDS_Client.RAGEAPI.Input;
using TDS_Client.RAGEAPI.Locale;
using TDS_Client.RAGEAPI.MapObject;
using TDS_Client.RAGEAPI.Misc;
using TDS_Client.RAGEAPI.Nametags;
using TDS_Client.RAGEAPI.Native;
using TDS_Client.RAGEAPI.Ped;
using TDS_Client.RAGEAPI.Player;
using TDS_Client.RAGEAPI.Pool;
using TDS_Client.RAGEAPI.Shapetest;
using TDS_Client.RAGEAPI.Stats;
using TDS_Client.RAGEAPI.Streaming;
using TDS_Client.RAGEAPI.Sync;
using TDS_Client.RAGEAPI.Ui;
using TDS_Client.RAGEAPI.Utils;
using TDS_Client.RAGEAPI.Vehicle;
using TDS_Client.RAGEAPI.Voice;
using TDS_Client.RAGEAPI.Weapon;
using TDS_Client.RAGEAPI.Windows;

namespace TDS_Client.RAGEAPI
{
    internal class BaseAPI : IRAGE.Game
    {
        #region Public Constructors

        public BaseAPI()
        {
            Pool = new PoolAPI();

            RAGE.Elements.Player.RecreateLocal();
            LocalPlayer = RAGE.Elements.Player.LocalPlayer as ITDSPlayer;

            Console = new ConsoleAPI();
            var loggingHandler = new LoggingHandler(this);

            Audio = new AudioAPI();
            Browser = new BrowserAPI();
            Blip = new BlipAPI();
            Cam = new CamAPI();
            Chat = new ChatAPI();
            Checkpoint = new CheckpointAPI();
            Control = new ControlAPI();
            Cursor = new CursorAPI();
            Cutscene = new CutsceneAPI();
            Discord = new DiscordAPI();
            Entity = new EntityAPI();
            Event = new EventAPI(loggingHandler);
            Graphics = new GraphicsAPI();
            Input = new InputAPI();
            Locale = new LocaleAPI();
            MapObject = new MapObjectAPI();
            Misc = new MiscAPI();
            Nametags = new NametagsAPI();
            Native = new NativeAPI();
            Ped = new PedAPI();
            Player = new PlayerAPI();

            Shapetest = new ShapetestAPI();
            Stats = new StatsAPI();
            Streaming = new StreamingAPI();
            Sync = new SyncAPI();
            Ui = new UiAPI();
            Utils = new UtilsAPI();
            Vehicle = new VehicleAPI();
            Voice = new VoiceAPI();
            Weapon = new WeaponAPI();
            Windows = new WindowsAPI();
        }

        #endregion Public Constructors

        #region Public Properties

        public IAudioAPI Audio { get; }
        public IBlipAPI Blip { get; }
        public IBrowserAPI Browser { get; }
        public ICamAPI Cam { get; }
        public IChatAPI Chat { get; }
        public ICheckpointAPI Checkpoint { get; }
        public IConsoleAPI Console { get; }
        public IControlAPI Control { get; }
        public ICursorAPI Cursor { get; }
        public ICutsceneAPI Cutscene { get; }
        public IDiscordAPI Discord { get; }
        public IEntityAPI Entity { get; }
        public IEventAPI Event { get; }
        public IGraphicsAPI Graphics { get; }
        public IInputAPI Input { get; }
        public ILocaleAPI Locale { get; }
        public ITDSPlayer LocalPlayer { get; }
        public IMapObjectAPI MapObject { get; }
        public IMiscAPI Misc { get; }
        public INametagsAPI Nametags { get; }
        public INativeAPI Native { get; }
        public IPedAPI Ped { get; }
        public ITDSPlayerAPI Player { get; }
        public IPoolAPI Pool { get; }
        public IShapetestAPI Shapetest { get; }
        public IStatsAPI Stats { get; }
        public IStreamingAPI Streaming { get; }
        public ISyncAPI Sync { get; }
        public IUiAPI Ui { get; }
        public IUtilsAPI Utils { get; }
        public IVehicleAPI Vehicle { get; }
        public IVoiceAPI Voice { get; }
        public IWeaponAPI Weapon { get; }
        public IWindowsAPI Windows { get; }

        #endregion Public Properties
    }
}