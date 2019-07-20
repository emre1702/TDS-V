using RAGE.Game;
using RAGE.NUI;
using System;
using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Instance.Draw.Dx
{
    internal class DxProgressRectangle : Dx
    {
        /// <summary>
        /// The progress between 0 and 1
        /// </summary>
        public float Progress
        {
            get => _progress;
            set => _progress = Math.Min(1, Math.Max(0, value));
        }

        private readonly float _width;

        private readonly bool _filling;
        private float _progress;
        private readonly bool _relativePos;
        private ulong? _msToEnd;
        private ulong _startTime;

        private readonly DxRectangle _backRect;
        private readonly DxRectangle _frontRect;
        private readonly DxText _text;

        public DxProgressRectangle(string text, float x, float y, float width, float height,
            Color textColor, Color backColor, Color progressColor,
            float textScale = 1.0f, Font textFont = Font.ChaletLondon, int frontRectOffsetAbsoluteX = 3, int frontRectOffsetAbsoluteY = 3, bool filling = true,
            UIResText.Alignment alignmentX = UIResText.Alignment.Centered, EAlignmentY alignmentY = EAlignmentY.Center, bool relativePos = true, int frontPriority = 0) 
            : base(frontPriority: frontPriority)
        {
            this._width = width;
            this._filling = filling;
            this._relativePos = relativePos;

            float textX = GetTextX(x, width, alignmentX);
            float textY = GetTextY(y, height, alignmentY);

            this._text = new DxText(text, textX, textY, textScale, textColor, textFont, UIResText.Alignment.Centered, EAlignmentY.Center, relativePos, false, true, frontPriority: frontPriority + 2);

            float frontRectX = GetFrontRectX(x, width, alignmentX, relativePos) + frontRectOffsetAbsoluteX;
            float frontRectY = GetAbsoluteY(y, relativePos);
            float frontRectWidth = GetAbsoluteX(width, relativePos) - frontRectOffsetAbsoluteX * 2;
            float frontRectHeight = GetAbsoluteY(height, relativePos) - frontRectOffsetAbsoluteY * 2;

            _backRect = new DxRectangle(x, y, width, height, backColor, alignmentX, alignmentY, relativePos, frontPriority: frontPriority);
            _frontRect = new DxRectangle(frontRectX, frontRectY, frontRectWidth, frontRectHeight, progressColor, UIResText.Alignment.Left, alignmentY, false, frontPriority: frontPriority + 1);

            Children.Add(_backRect);
            Children.Add(_frontRect);
            Children.Add(_text);
        }

        public void SetAutomatic(ulong msToEnd, bool restart = true)
        {
            if (restart)
                Progress = 0;
            _startTime = TimerManager.ElapsedTicks;
            _msToEnd = msToEnd;
            if (!restart)
            {
                uint progressMs = (uint)(msToEnd / Progress);
                _startTime -= progressMs;
            }
                
        }

        public override void Draw()
        {
            if (_msToEnd.HasValue)
            {
                float msWasted = TimerManager.ElapsedTicks - _startTime;
                Progress = msWasted / _msToEnd.Value;
            }
            
            if (_filling)
                _frontRect.SetWidth(_progress * _width, _relativePos);
            else
                _frontRect.SetWidth(_width - _width * _progress, _relativePos);
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