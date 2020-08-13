using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.TextLabel
{
    internal class TextLabelAPI : ITextLabelAPI
    {
        #region Public Methods

        public ITextLabel Create(string text, Position position, double range, float fontSize, int font, System.Drawing.Color color, bool entitySeethrough, ILobby lobby)
        {
            var instance = NAPI.TextLabel.CreateTextLabel(text, position.ToMod(), (float)range, fontSize, font, color.ToMod(), entitySeethrough, lobby.Dimension);

            var textLabel = new TextLabel(instance);
            return textLabel;
        }

        #endregion Public Methods
    }
}
