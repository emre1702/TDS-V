using RAGE.Game;
using System.Drawing;
using TDS_Client.Data.Enums;
using static RAGE.NUI.UIResText;
using Alignment = RAGE.NUI.UIResText.Alignment;

namespace TDS_Client.Handler.Draw.Dx
{
    internal class DxTextRectangle : DxBase
    {
        private readonly Alignment _alignment;
        private readonly AlignmentY _alignmentY;
        private readonly float _height;
        private readonly bool _relativePos;
        private readonly float _width;
        private readonly float _x;
        private readonly float _y;
        private DxRectangle _rect;
        private DxText _text;

        public DxTextRectangle(DxHandler dxHandler, TimerHandler timerHandler, string text, float x, float y, float width, float height,
            Color textColor, Color rectColor, float textScale = 1.0f, Font textFont = Font.ChaletLondon,
            int textOffsetAbsoluteX = 0,
            Alignment alignment = RAGE.NUI.UIResText.Alignment.Left, AlignmentY alignmentY = AlignmentY.Top,
            Alignment textAlignment = Alignment.Centered, AlignmentY textAlignmentY = AlignmentY.Center,
            bool relativePos = true,
            int amountLines = 0, bool activated = true, int frontPriority = 0) : base(dxHandler, frontPriority, activated)
        {
            _rect = new DxRectangle(dxHandler, x, y, width, height, rectColor, alignment, alignmentY, relativePos)
            {
                Activated = false
            };

            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
            this._relativePos = relativePos;

            this._alignment = alignment;
            this._alignmentY = alignmentY;

            float textX = relativePos ? GetTextRelativePosX(textOffsetAbsoluteX, textAlignment) : GetTextAbsolutePosX(textOffsetAbsoluteX, textAlignment);
            float textY = relativePos ? GetTextRelativePosY(textAlignmentY) : GetTextAbsolutePosY(textAlignmentY);
            this._text = new DxText(dxHandler, timerHandler, text, textX, textY, textScale, textColor, textFont, textAlignment, textAlignmentY, relativePos, amountLines: amountLines)
            {
                Activated = false
            };

            Children.Add(this._text);
            Children.Add(_rect);
        }

        public override void Draw()
        {
            _rect.Draw();
            _text.Draw();
        }

        public void SetText(string text)
        {
            this._text.Text = text;
            //this._text.SetAbsoluteY(GetAbsoluteY(_relativePos ? GetTextRelativePosY() : GetTextAbsolutePosY(), _relativePos));
        }

        private float GetRectangleLeftX()
        {
            switch (_alignment)
            {
                case Alignment.Centered:
                    return _x - _width / 2;

                case Alignment.Left:
                    return _x;

                case Alignment.Right:
                    return _x - _width;
            }
            return _x;
        }

        private float GetRectangleTopY()
        {
            switch (_alignmentY)
            {
                case AlignmentY.Center:
                    return _y - _height / 2;

                case AlignmentY.Top:
                    return _y;

                case AlignmentY.Bottom:
                    return _y - _height;
            }
            return _y;
        }

        private float GetTextAbsolutePosX(float offsetX, Alignment alignX)
        {
            float rectLeftX = GetRectangleLeftX();

            switch (alignX)
            {
                case Alignment.Centered:
                    return GetAbsoluteX(rectLeftX + _width / 2, _relativePos);

                case Alignment.Left:
                    return GetAbsoluteX(rectLeftX, _relativePos) + offsetX;

                case Alignment.Right:
                    return GetAbsoluteX(rectLeftX + _width, _relativePos) - offsetX;
            }
            return _x;
        }

        private float GetTextAbsolutePosY(AlignmentY alignY)
        {
            float rectTopY = GetRectangleTopY();

            switch (alignY)
            {
                case AlignmentY.Center:
                    return GetAbsoluteY(rectTopY + _height / 2, _relativePos);

                case AlignmentY.Top:
                    return GetAbsoluteY(rectTopY, _relativePos);

                case AlignmentY.Bottom:
                    return GetAbsoluteY(rectTopY + _height, _relativePos);
            }

            return _y;

            //int amountlines = textString.Count(t => t == '\n') + 1;
            //return y + height / 2 - GetAbsoluteY(Ui.GetTextScaleHeight(scale, (int)font), true) / 2 * amountlines;            //  - 5;
            //return _y + _height / 2 - 5;
        }

        private float GetTextRelativePosX(float offsetX, Alignment alignX)
        {
            if (_relativePos)
                offsetX = GetRelativeX(offsetX, false);
            float rectLeftX = GetRectangleLeftX();

            switch (alignX)
            {
                case Alignment.Centered:
                    return GetRelativeX(rectLeftX + _width / 2, _relativePos);

                case Alignment.Left:
                    return GetRelativeX(rectLeftX + offsetX, _relativePos);

                case Alignment.Right:
                    return GetRelativeX(rectLeftX + _width - offsetX, _relativePos);
            }
            return _x;
        }

        private float GetTextRelativePosY(AlignmentY alignY)
        {
            float rectTopY = GetRectangleTopY();

            switch (alignY)
            {
                case AlignmentY.Center:
                    return GetRelativeX(rectTopY + _height / 2, _relativePos);

                case AlignmentY.Top:
                    return GetRelativeX(rectTopY, _relativePos);

                case AlignmentY.Bottom:
                    return GetRelativeX(rectTopY + _height, _relativePos);
            }

            return _y;

            //int amountlines = textString.Count(t => t == '\n') + 1;
            //return y + height / 2 - Ui.GetTextScaleHeight(scale, (int)font) / 2 * amountlines;            // - GetRelativeY(5, false);
            //return _y + _height / 2 - GetRelativeY(5, false);
        }
    }
}
