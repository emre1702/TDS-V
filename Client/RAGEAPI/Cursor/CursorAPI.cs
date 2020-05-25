using TDS_Client.Data.Interfaces.ModAPI.Cursor;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Cursor
{
    internal class CursorAPI : ICursorAPI
    {
        #region Public Properties

        public Position2D Position
            => RAGE.Ui.Cursor.Position.ToPosition2D();

        public bool Visible
        {
            get => RAGE.Ui.Cursor.Visible;
            set => RAGE.Ui.Cursor.Visible = value;
        }

        #endregion Public Properties

        #region Public Methods

        public void ShowCursor(bool freezeGameInput, bool show)
        {
            RAGE.Ui.Cursor.ShowCursor(freezeGameInput, show);
        }

        #endregion Public Methods
    }
}
