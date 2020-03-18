using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;
using TDS_Server.RAGE.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGE.TextLabel
{
    class TextLabelAPI : ITextLabelAPI
    {
        public ITextLabel Create(string text, Position3D position, double range, float fontSize, int font, System.Drawing.Color color, bool entitySeethrough, ILobby lobby)
        {
            var instance = NAPI.TextLabel.CreateTextLabel(text, position.ToVector3(), (float)range, fontSize, font, color.ToColor(), entitySeethrough, lobby.Dimension);

            var textLabel = new TextLabel(instance);
            return textLabel;
        }
    }
}
