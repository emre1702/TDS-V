using RAGE.Game;
using RAGE.NUI;
using System.Drawing;
using System.Linq;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx
{
    internal class DxTextRectangle : Dx
    {
        private DxText text;
        private DxRectangle rect;

        private readonly float y;
        private readonly float height;
        private readonly string textString;
        private readonly Font font;
        private readonly float scale;
        private readonly bool relativePos;

        public DxTextRectangle(string text, float x, float y, float width, float height,
            Color textColor, Color rectColor, float textScale = 1.0f, Font textFont = Font.ChaletLondon,
            int textOffsetAbsoluteX = 0, UIResText.Alignment alignmentX = UIResText.Alignment.Left, EAlignmentY alignmentY = EAlignmentY.Top, bool relativePos = true,
            int amountLines = 1, bool activated = true, int frontPriority = 0) : base(frontPriority, activated)
        {
            rect = new DxRectangle(x, y, width, height, rectColor, alignmentX, alignmentY, relativePos)
            {
                Activated = false
            };

            this.y = y;
            this.height = height;
            textString = text;
            font = textFont;
            scale = textScale;
            this.relativePos = relativePos;
            float textY = relativePos ? GetTextRelativePosY() : GetTextAbsolutePosY();
            this.text = new DxText(text, x + (relativePos ? GetRelativeX(textOffsetAbsoluteX, false) : textOffsetAbsoluteX), textY, textScale, textColor, textFont, alignmentX, EAlignmentY.Center, relativePos, amountLines: amountLines)
            {
                Activated = false
            };

            Children.Add(this.text);
            Children.Add(rect);
        }

        private float GetTextRelativePosY()
        {
            int amountlines = textString.Count(t => t == '\n') + 1;
            return y + height / 2 - Ui.GetTextScaleHeight(scale, (int)font) / 2 * amountlines - GetRelativeY(5, false);
        }

        private float GetTextAbsolutePosY()
        {
            int amountlines = textString.Count(t => t == '\n') + 1;
            return y + height / 2 - GetAbsoluteY(Ui.GetTextScaleHeight(scale, (int)font), true) / 2 * amountlines - 5;
        }

        public override void Draw()
        {
            rect.Draw();
            text.Draw();
        }

        public void SetText(string text)
        {
            this.text.Text = text;
            this.text.Y = GetAbsoluteY(relativePos ? GetTextRelativePosY() : GetTextAbsolutePosY(), relativePos);
        }
    }
}