using RAGE.Game;
using RAGE.NUI;
using System;
using System.Drawing;
using TDS_Client.Enum;

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
            float textScale = 1.0f, Font textFont = Font.ChaletLondon, int textOffsetAbsoluteX = 1, bool filling = true,
            UIResText.Alignment alignmentX = UIResText.Alignment.Centered, EAlignmentY alignmentY = EAlignmentY.Center, bool relativePos = true) : base()
        {
            this.width = width;
            this.height = height;
            this.filling = filling;

            backTextRect = new DxTextRectangle(text, x, y, width, height, textColor, backColor, textScale, textFont, textOffsetAbsoluteX, alignmentX, alignmentY, relativePos);
            frontRect = new DxRectangle(x+1, y+1, 0, 0, progressColor, alignmentX, alignmentY, relativePos);
        }

        public override void Draw()
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
