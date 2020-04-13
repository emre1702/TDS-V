using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Audio;
using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Browser;
using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.Data.Interfaces.ModAPI.Chat;
using TDS_Client.Data.Interfaces.ModAPI.Console;
using TDS_Client.Data.Interfaces.ModAPI.Control;
using TDS_Client.Data.Interfaces.ModAPI.Cursor;
using TDS_Client.Data.Interfaces.ModAPI.Discord;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Graphics;
using TDS_Client.Data.Interfaces.ModAPI.Input;
using TDS_Client.Data.Interfaces.ModAPI.Locale;
using TDS_Client.Data.Interfaces.ModAPI.MapObject;
using TDS_Client.Data.Interfaces.ModAPI.Misc;
using TDS_Client.Data.Interfaces.ModAPI.Nametags;
using TDS_Client.Data.Interfaces.ModAPI.Native;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.Data.Interfaces.ModAPI.Shapetest;
using TDS_Client.Data.Interfaces.ModAPI.Stats;
using TDS_Client.Data.Interfaces.ModAPI.Streaming;
using TDS_Client.Data.Interfaces.ModAPI.Sync;
using TDS_Client.Data.Interfaces.ModAPI.Ui;
using TDS_Client.Data.Interfaces.ModAPI.Utils;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.Data.Interfaces.ModAPI.Voice;
using TDS_Client.Data.Interfaces.ModAPI.Weapon;
using TDS_Client.RAGEAPI.Audio;
using TDS_Client.RAGEAPI.Blip;
using TDS_Client.RAGEAPI.Browser;
using TDS_Client.RAGEAPI.Cam;
using TDS_Client.RAGEAPI.Chat;
using TDS_Client.RAGEAPI.Console;
using TDS_Client.RAGEAPI.Control;
using TDS_Client.RAGEAPI.Cursor;
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

namespace TDS_Client.RAGEAPI
{
    class BaseAPI : IModAPI
    {
        public IAudioAPI Audio { get; }
        public IBrowserAPI Browser { get; }
        public IBlipAPI Blip { get; }
        public ICamAPI Cam { get; }
        public IChatAPI Chat { get; }
        public IConsoleAPI Console { get; }
        public IControlAPI Control { get; }
        public ICursorAPI Cursor { get; }
        public IDiscordAPI Discord { get; }
        public IEntityAPI Entity { get; }
        public IEventAPI Event { get; }
        public IGraphicsAPI Graphics { get; }
        public IInputAPI Input { get; }
        public ILocaleAPI Locale { get; }
        public IMapObjectAPI MapObject { get; }
        public IMiscAPI Misc { get; }
        public INametagsAPI Nametags { get; }
        public INativeAPI Native { get; }
        public IPedAPI Ped { get; }
        public IPlayerAPI Player { get; }
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

        public IPlayer LocalPlayer { get; }

        public BaseAPI()
        {
            var playerConvertingHandler = new PlayerConvertingHandler();
            var entityConvertingHandler = new EntityConvertingHandler(playerConvertingHandler);

            Audio = new AudioAPI();
            Browser = new BrowserAPI();
            Blip = new BlipAPI(entityConvertingHandler);
            Cam = new CamAPI();
            Chat = new ChatAPI();
            Console = new ConsoleAPI();
            Control = new ControlAPI();
            Cursor = new CursorAPI();
            Discord = new DiscordAPI();
            Entity = new EntityAPI();
            Event = new EventAPI(playerConvertingHandler, entityConvertingHandler);
            Graphics = new GraphicsAPI();
            Input = new InputAPI();
            Locale = new LocaleAPI();
            MapObject = new MapObjectAPI(entityConvertingHandler);
            Misc = new MiscAPI();
            Nametags = new NametagsAPI();
            Native = new NativeAPI();
            Ped = new PedAPI(entityConvertingHandler);
            Player = new PlayerAPI();
            Pool = new PoolAPI(entityConvertingHandler);
            Shapetest = new ShapetestAPI();
            Stats = new StatsAPI();
            Streaming = new StreamingAPI();
            Sync = new SyncAPI();
            Ui = new UiAPI();
            Utils = new UtilsAPI();
            Vehicle = new VehicleAPI(entityConvertingHandler);
            Voice = new VoiceAPI();
            Weapon = new WeaponAPI();

            LocalPlayer = playerConvertingHandler.GetPlayer(RAGE.Elements.Player.LocalPlayer);
        }
    }
}
