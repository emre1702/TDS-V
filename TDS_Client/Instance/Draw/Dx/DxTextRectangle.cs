using RAGE.Game;
using RAGE.NUI;
using System.Drawing;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx
{
    class DxTextRectangle : Dx
    {
        private DxText text;
        private DxRectangle rect;

        public DxTextRectangle(string text, float x, float y, float width, float height,
            Color textColor, Color rectColor, float textScale = 1.0f, Font textFont = Font.ChaletLondon,
            int textOffsetAbsoluteX = 1, UIResText.Alignment alignmentX = UIResText.Alignment.Left, EAlignmentY alignmentY = EAlignmentY.Top, bool relativePos = true, bool activated = true) : base(activated)
        {
            rect = new DxRectangle(x, y, width, height, rectColor, alignmentX, alignmentY, relativePos)
            {
                Activated = false
            };
            this.text = new DxText(text, x + (relativePos ? GetRelativeX(textOffsetAbsoluteX, false) : textOffsetAbsoluteX), y, textScale, textColor, textFont, alignmentX, alignmentY, relativePos)
            {
                Activated = false
            };
        }

        public override void Draw()
        {
            rect.Draw();
            text.Draw();
        }

        public void SetText(string text)
        {
            this.text.Text = text;
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
