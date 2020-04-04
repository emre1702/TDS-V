using TDS_Client.Data.Interfaces.ModAPI.Browser;
using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.Data.Interfaces.ModAPI.Chat;
using TDS_Client.Data.Interfaces.ModAPI.Control;
using TDS_Client.Data.Interfaces.ModAPI.Cursor;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Graphics;
using TDS_Client.Data.Interfaces.ModAPI.Native;
using TDS_Client.Data.Interfaces.ModAPI.Sync;
using TDS_Client.Data.Interfaces.ModAPI.Ui;
using TDS_Client.Data.Interfaces.ModAPI.Utils;

namespace TDS_Client.Data.Interfaces.ModAPI
{
    public interface IModAPI
    {
        IBrowserAPI Browser { get; }
        ICamAPI Cam { get; }
        IChatAPI Chat { get; }
        IControlAPI Control { get; }
        ICursorAPI Cursor { get; }
        IEventAPI Event { get; }
        IGraphicsAPI Graphics { get; }
        INativeAPI Native { get; }
        IPoolAPI Pool { get; }
        ISyncAPI Sync { get; }
        IUiAPI Ui { get; }
        IUtilsAPI Utils { get; }
    }
}
