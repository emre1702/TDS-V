using TDS_Client.Data.Interfaces.ModAPI.Browser;
using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.Data.Interfaces.ModAPI.Chat;
using TDS_Client.Data.Interfaces.ModAPI.Control;
using TDS_Client.Data.Interfaces.ModAPI.Cursor;
using TDS_Client.Data.Interfaces.ModAPI.Discord;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Graphics;
using TDS_Client.Data.Interfaces.ModAPI.Input;
using TDS_Client.Data.Interfaces.ModAPI.Locale;
using TDS_Client.Data.Interfaces.ModAPI.Nametags;
using TDS_Client.Data.Interfaces.ModAPI.Native;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Interfaces.ModAPI.Stats;
using TDS_Client.Data.Interfaces.ModAPI.Streaming;
using TDS_Client.Data.Interfaces.ModAPI.Sync;
using TDS_Client.Data.Interfaces.ModAPI.Ui;
using TDS_Client.Data.Interfaces.ModAPI.Utils;
using TDS_Client.Data.Interfaces.ModAPI.Voice;

namespace TDS_Client.Data.Interfaces.ModAPI
{
    public interface IModAPI
    {
        IBrowserAPI Browser { get; }
        ICamAPI Cam { get; }
        IChatAPI Chat { get; }
        IControlAPI Control { get; }
        ICursorAPI Cursor { get; }
        IDiscordAPI Discord { get; }
        IEventAPI Event { get; }
        IGraphicsAPI Graphics { get; }
        IInputAPI Input { get; }
        ILocaleAPI Locale { get; }
        INametagsAPI Nametags { get; }
        INativeAPI Native { get; }
        IPoolAPI Pool { get; }
        IStatsAPI Stats { get; }
        IStreamingAPI Streaming { get; }
        ISyncAPI Sync { get; }
        IUiAPI Ui { get; }
        IUtilsAPI Utils { get; }
        IVoiceAPI Voice { get; }

        IPlayer LocalPlayer { get; }
        
    }
}
