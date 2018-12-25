using RAGE.Game;
using System.Drawing;

namespace TDS_Client.Instance.Draw.Dx
{
    class DxTextRectangle : Dx
    {
        private DxText text;
        private DxRectangle rect;

        public DxTextRectangle(string text, float x, float y, float width, float height,
            Color textColor, Color rectColor, float textScale = 1.0f, Font textFont = Font.ChaletLondon,
            int textOffsetAbsoluteX = 1, Alignment alignment = Alignment.Left, bool relativePos = true, bool activated = true) : base(activated)
        {
            rect = new DxRectangle(x, y, width, height, rectColor, alignment, relativePos);
            this.text = new DxText(text, x + textOffsetAbsoluteX, y, textScale, textColor, textFont, alignment, relativePos);
        }

        public void SetText(string text)
        {
            this.text.SetText(text);
        }

        public override void Remove()
        {
            base.Remove();
            text.Remove();
            rect.Remove();
            text = null;
            rect = null;
        }
    }
}
