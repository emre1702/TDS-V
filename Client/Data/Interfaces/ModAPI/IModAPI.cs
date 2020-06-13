using TDS_Client.Data.Interfaces.ModAPI.Audio;
using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Browser;
using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.Data.Interfaces.ModAPI.Chat;
using TDS_Client.Data.Interfaces.ModAPI.Checkpoint;
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
using TDS_Client.Data.Interfaces.ModAPI.Windows;

namespace TDS_Client.Data.Interfaces.ModAPI
{
    public interface IModAPI
    {
        #region Public Properties

        IAudioAPI Audio { get; }
        IBlipAPI Blip { get; }
        IBrowserAPI Browser { get; }
        ICamAPI Cam { get; }
        IChatAPI Chat { get; }
        ICheckpointAPI Checkpoint { get; }
        IConsoleAPI Console { get; }
        IControlAPI Control { get; }
        ICursorAPI Cursor { get; }
        IDiscordAPI Discord { get; }
        IEntityAPI Entity { get; }
        IEventAPI Event { get; }
        IGraphicsAPI Graphics { get; }
        IInputAPI Input { get; }
        ILocaleAPI Locale { get; }
        IPlayer LocalPlayer { get; }
        IMapObjectAPI MapObject { get; }
        IMiscAPI Misc { get; }
        INametagsAPI Nametags { get; }
        INativeAPI Native { get; }
        IPedAPI Ped { get; }
        IPlayerAPI Player { get; }
        IPoolAPI Pool { get; }
        IShapetestAPI Shapetest { get; }
        IStatsAPI Stats { get; }
        IStreamingAPI Streaming { get; }
        ISyncAPI Sync { get; }
        IUiAPI Ui { get; }
        IUtilsAPI Utils { get; }
        IVehicleAPI Vehicle { get; }
        IVoiceAPI Voice { get; }
        IWeaponAPI Weapon { get; }
        IWindowsAPI Windows { get; }

        #endregion Public Properties
    }
}
