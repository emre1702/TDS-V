using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Cursor
{
    public interface ICursorAPI
    {
        Position2D Position { get; }
        bool Visible { get; set; }

        void ShowCursor(bool freezeGameInput, bool show);
    }
}
