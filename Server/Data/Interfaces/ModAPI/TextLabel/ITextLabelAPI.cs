using System.Drawing;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.TextLabel
{
    public interface ITextLabelAPI
    {
        #region Public Methods

        ITextLabel Create(string text, Position3D position, double range, float fontSize, int font, Color color, bool entitySeethrough, ILobby lobby);

        #endregion Public Methods
    }
}
