using RAGE.Game;
using System;
using System.Drawing;

namespace TDS_Client.Instance.Draw.Dx
{
    class DxProgressRectangle : Dx
    {
        private float width;
        private float height;

        private bool filling;
        private float progress;

        private DxTextRectangle backTextRect;
        private DxRectangle frontRect;

        /// <summary>
        /// The progress between 0 and 1
        /// </summary>
        public float Progress {
            get => progress;
            set => progress = Math.Min(0, Math.Max(1, value));
        }

        public DxProgressRectangle(string text, float x, float y, float width, float height,
            Color textColor, Color backColor, Color progressColor,
            float textScale = 1.0f, Font textFont = Font.ChaletLondon, int textOffsetAbsoluteX = 1, bool filling = true, Alignment alignment = Alignment.Left, bool relativePos = true) : base()
        {
            backTextRect = new DxTextRectangle(text, x, y, width, height, textColor, backColor, textScale, textFont, textOffsetAbsoluteX, alignment, relativePos);
            frontRect = new DxRectangle(x+1, y+1, 0, 0, progressColor, alignment, relativePos);
        }

        protected override void Draw(int tick)
        {
            if (filling)
                frontRect.SetWidth(progress * width);
            else
                frontRect.SetWidth(width - width * progress);
        }

        public override void Remove()
        {
            base.Remove();
            backTextRect.Remove();
            frontRect.Remove();
            backTextRect = null;
            frontRect = null;
        }
    }
}
