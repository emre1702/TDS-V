using RAGE;
using RAGE.Game;
using RAGE.NUI;
using System;
using System.Drawing;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx
{
    class DxProgressRectangle : Dx
    {
        private readonly float width;

        private readonly bool filling;
        private float progress;
        private readonly bool relativePos;

        private DxRectangle backRect;
        private DxRectangle frontRect;
        private DxText text;

        /// <summary>
        /// The progress between 0 and 1
        /// </summary>
        public float Progress {
            get => progress;
            set => progress = Math.Min(1, Math.Max(0, value));
        }

        public DxProgressRectangle(string text, float x, float y, float width, float height,
            Color textColor, Color backColor, Color progressColor,
            float textScale = 1.0f, Font textFont = Font.ChaletLondon, int frontRectOffsetAbsoluteX = 3, int frontRectOffsetAbsoluteY = 3, bool filling = true,
            UIResText.Alignment alignmentX = UIResText.Alignment.Centered, EAlignmentY alignmentY = EAlignmentY.Center, bool relativePos = true) : base()
        {
            this.width = width;
            this.filling = filling;
            this.relativePos = relativePos;

            float textX = GetTextX(x, width, alignmentX);
            float textY = GetTextY(y, height, alignmentY);

            this.text = new DxText(text, textX, textY, textScale, textColor, textFont, UIResText.Alignment.Centered, EAlignmentY.Center, relativePos, false, true);

            float frontRectX = GetFrontRectX(x, width, alignmentX, relativePos) + frontRectOffsetAbsoluteX;
            float frontRectY = GetAbsoluteY(y, relativePos);
            float frontRectWidth = GetAbsoluteX(width, relativePos) - frontRectOffsetAbsoluteX * 2;
            float frontRectHeight = GetAbsoluteY(height, relativePos) - frontRectOffsetAbsoluteY * 2;
            frontRect = new DxRectangle(frontRectX, frontRectY, frontRectWidth, frontRectHeight, progressColor, UIResText.Alignment.Left, alignmentY, false);

            backRect = new DxRectangle(x, y, width, height, backColor, alignmentX, alignmentY, relativePos);

            children.Add(frontRect);
            children.Add(backRect);
            children.Add(this.text);
        }

        public override void Draw()
        {
            if (filling)
                frontRect.SetWidth(progress * width, relativePos);
            else
                frontRect.SetWidth(width - width * progress, relativePos);
        }

        private float GetFrontRectX(float x, float width, UIResText.Alignment alignment, bool relativePos)
        {
            if (alignment == UIResText.Alignment.Centered)
                return GetAbsoluteX(x - width / 2, relativePos);
            else if (alignment == UIResText.Alignment.Left)
                return GetAbsoluteX(x, relativePos);
            else
                return GetAbsoluteX(x - width, relativePos);
        }

        private float GetTextX(float x, float width, UIResText.Alignment alignment)
        {
            if (alignment == UIResText.Alignment.Centered)
                return x;
            else if (alignment == UIResText.Alignment.Left)
                return x + width / 2;
            else
                return x - width / 2;
        }

        private float GetTextY(float y, float height, EAlignmentY alignment)
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
