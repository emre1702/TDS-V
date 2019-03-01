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

        private DxRectangle backRect;
        private DxRectangle frontRect;
        private DxText text;

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

            backRect = new DxRectangle(x, y, width, height, backColor, alignmentX, alignmentY, relativePos);
            frontRect = new DxRectangle(x+1, y+1, 0, height-2, progressColor, alignmentX, alignmentY, relativePos);

            float textX = getTextX(x, width, alignmentX);
            float textY = getTextY(y, height, alignmentY);

            this.text = new DxText(text, textX, textY, textScale, textColor, textFont, UIResText.Alignment.Centered, EAlignmentY.Center, relativePos, false, true);
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
            backRect.Remove();
            frontRect.Remove();
            text.Remove();
            backRect = null;
            frontRect = null;
            text = null;
        }

        private float getTextX(float x, float width, UIResText.Alignment alignment)
        {
            if (alignment == UIResText.Alignment.Centered)
                return x;
            else if (alignment == UIResText.Alignment.Left)
                return x + width / 2;
            else
                return x - width / 2;
        }

        private float getTextY(float y, float height, EAlignmentY alignment)
        {
            if (alignment == EAlignmentY.Center)
                return y;
            else if (alignment == EAlignmentY.Top)
                return y + height / 2;
            else
                return y - height / 2;
        }
    }
}
