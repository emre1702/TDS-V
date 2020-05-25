using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Cursor
{
    public interface ICursorAPI
    {
        #region Public Properties

        Position2D Position { get; }
        bool Visible { get; set; }

        #endregion Public Properties

        #region Public Methods

        void ShowCursor(bool freezeGameInput, bool show);

        #endregion Public Methods
    }
}
