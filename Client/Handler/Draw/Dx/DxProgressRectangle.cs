using RAGE.Game;
using System;
using System.Drawing;
using TDS.Client.Data.Enums;
using static RAGE.NUI.UIResText;
using Alignment = RAGE.NUI.UIResText.Alignment;

namespace TDS.Client.Handler.Draw.Dx
{
    internal class DxProgressRectangle : DxBase
    {
        private readonly DxRectangle _backRect;

        private readonly bool _filling;

        private readonly DxRectangle _frontRect;

        private readonly bool _relativePos;

        private readonly DxText _text;

        private readonly TimerHandler _timerHandler;

        private readonly float _width;

        private readonly int _frontRectOffsetAbsoluteX;

        private int? _msToEnd;

        private float _progress;

        private int _startTime;

        public DxProgressRectangle(DxHandler dxHandler, TimerHandler timerHandler, string text, float x, float y, float width, float height,
                    Color textColor, Color backColor, Color progressColor,
                    float textScale = 1.0f, Font textFont = Font.ChaletLondon, int frontRectOffsetAbsoluteX = 3, int frontRectOffsetAbsoluteY = 3, bool filling = true,
                    Alignment alignmentX = Alignment.Centered, AlignmentY alignmentY = AlignmentY.Center, bool relativePos = true, int frontPriority = 0)
                    : base(dxHandler, frontPriority: frontPriority)
        {
            _timerHandler = timerHandler;

            this._width = width;
            this._filling = filling;
            this._relativePos = relativePos;
            _frontRectOffsetAbsoluteX = frontRectOffsetAbsoluteX;

            float textX = GetTextX(x, width, alignmentX);
            float textY = GetTextY(y, height, alignmentY);

            this._text = new DxText(dxHandler, timerHandler, text, textX, textY, textScale, textColor, textFont, Alignment.Centered, AlignmentY.Center, relativePos, false, true,
                frontPriority: frontPriority + 2);

            float frontRectX = GetFrontRectX(x, width, alignmentX, relativePos) + frontRectOffsetAbsoluteX;
            float frontRectY = GetAbsoluteY(y, relativePos);
            float frontRectWidth = GetAbsoluteX(width, relativePos) - frontRectOffsetAbsoluteX * 2;
            float frontRectHeight = GetAbsoluteY(height, relativePos) - frontRectOffsetAbsoluteY * 2;

            _backRect = new DxRectangle(dxHandler, x, y, width, height, backColor, alignmentX, alignmentY, relativePos, frontPriority: frontPriority);
            _frontRect = new DxRectangle(dxHandler, frontRectX, frontRectY, frontRectWidth, frontRectHeight, progressColor, Alignment.Left, alignmentY, false, frontPriority: frontPriority + 1);

            Children.Add(_backRect);
            Children.Add(_frontRect);
            Children.Add(_text);
        }

        /// <summary>
        /// The progress between 0 and 1
        /// </summary>
        public float Progress
        {
            get => _progress;
            set => _progress = Math.Min(1, Math.Max(0, value));
        }

        public override void Draw()
        {
            if (_msToEnd.HasValue)
            {
                float progressMs = _timerHandler.ElapsedMs - _startTime;
                Progress = progressMs / _msToEnd.Value;
            }

            float width = GetAbsoluteX(_width, _relativePos) - _frontRectOffsetAbsoluteX * 2;
            if (_filling)
                _frontRect.SetWidth(_progress * width, false);
            else
                _frontRect.SetWidth(width - width * _progress, false);
        }

        public void SetAutomatic(int msToEnd, bool restart = true)
        {
            if (restart)
                Progress = 0;
            _startTime = _timerHandler.ElapsedMs;
            _msToEnd = msToEnd;
            if (!restart)
            {
                int progressMs = (int)(msToEnd / Progress);
                _startTime -= progressMs;
            }
        }

        private float GetFrontRectX(float x, float width, Alignment alignment, bool relativePos)
        {
            if (alignment == Alignment.Centered)
                return GetAbsoluteX(x - width / 2, relativePos);
            else if (alignment == Alignment.Left)
                return GetAbsoluteX(x, relativePos);
            else
                return GetAbsoluteX(x - width, relativePos);
        }

        private float GetTextX(float x, float width, Alignment alignment)
        {
            if (alignment == Alignment.Centered)
                return x;
            else if (alignment == Alignment.Left)
                return x + width / 2;
            else
                return x - width / 2;
        }

        private float GetTextY(float y, float height, AlignmentY alignment)
        {
            if (alignment == AlignmentY.Center)
                return y;
            else if (alignment == AlignmentY.Top)
                return y + height / 2;
            else
                return y - height / 2;
        }
    }
}
