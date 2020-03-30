using TDS_Client.Data.Interfaces.ModAPI.Browser;
using TDS_Client.Data.Interfaces.ModAPI.Cursor;
using TDS_Client.Data.Interfaces.ModAPI.Native;

namespace TDS_Client.Data.Interfaces.ModAPI
{
    public interface IModAPI
    {
        IBrowserAPI Browser { get; }
        ICursorAPI Cursor { get; }
        INativeAPI Native { get; }
    }
}
