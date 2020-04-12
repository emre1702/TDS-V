using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.Draw.Dx
{
    internal class DxTextRectangle : DxBase
    {
        private DxText _text;
        private DxRectangle _rect;

        private readonly float _x;
        private readonly float _y;
        private readonly float _width;
        private readonly float _height;
        private readonly bool _relativePos;

        private readonly AlignmentX _alignmentX;
        private readonly AlignmentY _alignmentY;

        public DxTextRectangle(DxHandler dxHandler, IModAPI modAPI, TimerHandler timerHandler, string text, float x, float y, float width, float height,
            Color textColor, Color rectColor, float textScale = 1.0f, Font textFont = Font.ChaletLondon,
            int textOffsetAbsoluteX = 0,
            AlignmentX alignmentX = AlignmentX.Left, AlignmentY alignmentY = AlignmentY.Top,
            AlignmentX textAlignmentX = AlignmentX.Center, AlignmentY textAlignmentY = AlignmentY.Center,
            bool relativePos = true,
            int amountLines = 0, bool activated = true, int frontPriority = 0) : base(dxHandler, modAPI, frontPriority, activated)
        {
            _rect = new DxRectangle(dxHandler, modAPI, x, y, width, height, rectColor, alignmentX, alignmentY, relativePos)
            {
                Activated = false
            };

            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
            this._relativePos = relativePos;

            this._alignmentX = alignmentX;
            this._alignmentY = alignmentY;

            float textX = relativePos ? GetTextRelativePosX(textOffsetAbsoluteX, textAlignmentX) : GetTextAbsolutePosX(textOffsetAbsoluteX, textAlignmentX);
            float textY = relativePos ? GetTextRelativePosY(textAlignmentY) : GetTextAbsolutePosY(textAlignmentY);
            this._text = new DxText(dxHandler, modAPI, timerHandler, text, textX, textY, textScale, textColor, textFont, textAlignmentX, textAlignmentY, relativePos, amountLines: amountLines)
            {
                Activated = false
            };

            Children.Add(this._text);
            Children.Add(_rect);
        }

        private float GetTextRelativePosX(float offsetX, AlignmentX alignX)
        {
            if (_relativePos)
                offsetX = GetRelativeX(offsetX, false);
            float rectLeftX = GetRectangleLeftX();

            switch (alignX)
            {
                case AlignmentX.Center:
                    return GetRelativeX(rectLeftX + _width / 2, _relativePos);
                case AlignmentX.Left:
                    return GetRelativeX(rectLeftX + offsetX, _relativePos);
                case AlignmentX.Right:
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

        private float GetTextAbsolutePosX(float offsetX, AlignmentX alignX)
        {
            float rectLeftX = GetRectangleLeftX();

            switch (alignX)
            {
                case AlignmentX.Center:
                    return GetAbsoluteX(rectLeftX + _width / 2, _relativePos);
                case AlignmentX.Left:
                    return GetAbsoluteX(rectLeftX, _relativePos) + offsetX;
                case AlignmentX.Right:
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

        private float GetRectangleLeftX()
        {
            switch (_alignmentX)
            {
                case AlignmentX.Center:
                    return _x - _width / 2;
                case AlignmentX.Left:
                    return _x;
                case AlignmentX.Right:
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
    }
}
