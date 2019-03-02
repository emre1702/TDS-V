using RAGE.Game;
using RAGE.NUI;
using System.Drawing;
using System.Linq;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx
{
    class DxTextRectangle : Dx
    {
        private DxText text;
        private DxRectangle rect;

        private float x;
        private float y;
        private float width;
        private float height;
        private string textString;
        private Font font;
        private float scale;
        private bool relativePos;

        public DxTextRectangle(string text, float x, float y, float width, float height,
            Color textColor, Color rectColor, float textScale = 1.0f, Font textFont = Font.ChaletLondon,
            int textOffsetAbsoluteX = 0, UIResText.Alignment alignmentX = UIResText.Alignment.Left, EAlignmentY alignmentY = EAlignmentY.Top, bool relativePos = true, 
            int amountLines = 1, bool activated = true) : base(activated)
        {
            rect = new DxRectangle(x, y, width, height, rectColor, alignmentX, alignmentY, relativePos)
            {
                Activated = false
            };

            this.x = x;
            this.y = y;
            this.width = width;
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

        public override void Remove()
        {
            base.Remove();
            text?.Remove();
            rect?.Remove();
            text = null;
            rect = null;
        }
    }
}
