using RAGE.Game;
using RAGE.NUI;
using System.Drawing;
using TDS_Client.Enum;

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

        private readonly UIResText.Alignment _alignmentX;
        private readonly ToServerEvent _ToServerEvent;

        public DxTextRectangle(string text, float x, float y, float width, float height,
            Color textColor, Color rectColor, float textScale = 1.0f, Font textFont = Font.ChaletLondon,
            int textOffsetAbsoluteX = 0, 
            UIResText.Alignment alignmentX = UIResText.Alignment.Left, ToServerEvent ToServerEvent = ToServerEvent.Top,
            UIResText.Alignment textAlignmentX = UIResText.Alignment.Centered, ToServerEvent textToServerEvent = ToServerEvent.Center,
            bool relativePos = true,
            int amountLines = 0, bool activated = true, int frontPriority = 0) : base(frontPriority, activated)
        {
            _rect = new DxRectangle(x, y, width, height, rectColor, alignmentX, ToServerEvent, relativePos)
            {
                Activated = false
            };

            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
            this._relativePos = relativePos;

            this._alignmentX = alignmentX;
            this._ToServerEvent = ToServerEvent;

            float textX = relativePos ? GetTextRelativePosX(textOffsetAbsoluteX, textAlignmentX) : GetTextAbsolutePosX(textOffsetAbsoluteX, textAlignmentX);
            float textY = relativePos ? GetTextRelativePosY(textToServerEvent) : GetTextAbsolutePosY(textToServerEvent);
            this._text = new DxText(text, textX, textY, textScale, textColor, textFont, textAlignmentX, textToServerEvent, relativePos, amountLines: amountLines)
            {
                Activated = false
            };

            Children.Add(this._text);
            Children.Add(_rect);
        }

        private float GetTextRelativePosX(float offsetX, UIResText.Alignment alignX)
        {
            if (_relativePos)
                offsetX = GetRelativeX(offsetX, false);
            float rectLeftX = GetRectangleLeftX();

            switch (alignX)
            {
                case UIResText.Alignment.Centered:
                    return GetRelativeX(rectLeftX + _width / 2, _relativePos);
                case UIResText.Alignment.Left:
                    return GetRelativeX(rectLeftX + offsetX, _relativePos);
                case UIResText.Alignment.Right:
                    return GetRelativeX(rectLeftX + _width - offsetX, _relativePos);
            }
            return _x;
        }

        private float GetTextRelativePosY(ToServerEvent alignY)
        {
            float rectTopY = GetRectangleTopY();

            switch (alignY)
            {
                case ToServerEvent.Center:
                    return GetRelativeX(rectTopY + _height / 2, _relativePos);
                case ToServerEvent.Top:
                    return GetRelativeX(rectTopY, _relativePos);
                case ToServerEvent.Bottom:
                    return GetRelativeX(rectTopY + _height, _relativePos);
            }

            return _y;

            //int amountlines = textString.Count(t => t == '\n') + 1;
            //return y + height / 2 - Ui.GetTextScaleHeight(scale, (int)font) / 2 * amountlines;            // - GetRelativeY(5, false);
            //return _y + _height / 2 - GetRelativeY(5, false);
        }

        private float GetTextAbsolutePosX(float offsetX, UIResText.Alignment alignX)
        {
            float rectLeftX = GetRectangleLeftX();

            switch (alignX)
            {
                case UIResText.Alignment.Centered:
                    return GetAbsoluteX(rectLeftX + _width / 2, _relativePos);
                case UIResText.Alignment.Left:
                    return GetAbsoluteX(rectLeftX, _relativePos) + offsetX;
                case UIResText.Alignment.Right:
                    return GetAbsoluteX(rectLeftX + _width, _relativePos) - offsetX;
            }
            return _x;
        }

        private float GetTextAbsolutePosY(ToServerEvent alignY)
        {
            float rectTopY = GetRectangleTopY();

            switch (alignY)
            {
                case ToServerEvent.Center:
                    return GetAbsoluteY(rectTopY + _height / 2, _relativePos);
                case ToServerEvent.Top:
                    return GetAbsoluteY(rectTopY, _relativePos);
                case ToServerEvent.Bottom:
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
                case UIResText.Alignment.Centered:
                    return _x - _width / 2;
                case UIResText.Alignment.Left:
                    return _x;
                case UIResText.Alignment.Right:
                    return _x - _width;
            }
            return _x;
        }

        private float GetRectangleTopY()
        {
            switch (_ToServerEvent)
            {
                case ToServerEvent.Center:
                    return _y - _height / 2;
                case ToServerEvent.Top:
                    return _y;
                case ToServerEvent.Bottom:
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
