using TDS_Client.Data.Interfaces.ModAPI.Cursor;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Cursor
{
    class CursorAPI : ICursorAPI
    {
        public bool Visible 
        { 
            get => RAGE.Ui.Cursor.Visible; 
            set => RAGE.Ui.Cursor.Visible = true; 
        }

        public Position2D Position
            => RAGE.Ui.Cursor.Position.ToPosition2D();

        public void ShowCursor(bool freezeGameInput, bool show)
        {
            RAGE.Ui.Cursor.ShowCursor(freezeGameInput, show);
        }
    }
}
