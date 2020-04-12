﻿using System;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.Draw.Dx
{
    internal class DxProgressRectangle : DxBase
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

        private readonly TimerHandler _timerHandler;

        public DxProgressRectangle(DxHandler dxHandler, IModAPI modAPI, TimerHandler timerHandler, string text, float x, float y, float width, float height,
            Color textColor, Color backColor, Color progressColor,
            float textScale = 1.0f, Font textFont = Font.ChaletLondon, int frontRectOffsetAbsoluteX = 3, int frontRectOffsetAbsoluteY = 3, bool filling = true,
            AlignmentX alignmentX = AlignmentX.Center, AlignmentY alignmentY = AlignmentY.Center, bool relativePos = true, int frontPriority = 0)
            : base(dxHandler, modAPI, frontPriority: frontPriority)
        {
            _timerHandler = timerHandler;

            this._width = width;
            this._filling = filling;
            this._relativePos = relativePos;

            float textX = GetTextX(x, width, alignmentX);
            float textY = GetTextY(y, height, alignmentY);

            this._text = new DxText(dxHandler, modAPI, timerHandler, text, textX, textY, textScale, textColor, textFont, AlignmentX.Center, AlignmentY.Center, relativePos, false, true,
                frontPriority: frontPriority + 2);

            float frontRectX = GetFrontRectX(x, width, alignmentX, relativePos) + frontRectOffsetAbsoluteX;
            float frontRectY = GetAbsoluteY(y, relativePos);
            float frontRectWidth = GetAbsoluteX(width, relativePos) - frontRectOffsetAbsoluteX * 2;
            float frontRectHeight = GetAbsoluteY(height, relativePos) - frontRectOffsetAbsoluteY * 2;

            _backRect = new DxRectangle(dxHandler, modAPI, x, y, width, height, backColor, alignmentX, alignmentY, relativePos, frontPriority: frontPriority);
            _frontRect = new DxRectangle(dxHandler, modAPI, frontRectX, frontRectY, frontRectWidth, frontRectHeight, progressColor, AlignmentX.Left, alignmentY, false, frontPriority: frontPriority + 1);

            Children.Add(_backRect);
            Children.Add(_frontRect);
            Children.Add(_text);
        }

        public void SetAutomatic(ulong msToEnd, bool restart = true)
        {
            if (restart)
                Progress = 0;
            _startTime = _timerHandler.ElapsedMs;
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
                float msWasted = _timerHandler.ElapsedMs - _startTime;
                Progress = msWasted / _msToEnd.Value;
            }

            if (_filling)
                _frontRect.SetWidth(_progress * _width, _relativePos);
            else
                _frontRect.SetWidth(_width - _width * _progress, _relativePos);
        }

        private float GetFrontRectX(float x, float width, AlignmentX alignment, bool relativePos)
        {
            if (alignment == AlignmentX.Center)
                return GetAbsoluteX(x - width / 2, relativePos);
            else if (alignment == AlignmentX.Left)
                return GetAbsoluteX(x, relativePos);
            else
                return GetAbsoluteX(x - width, relativePos);
        }

        private float GetTextX(float x, float width, AlignmentX alignment)
        {
            if (alignment == AlignmentX.Center)
                return x;
            else if (alignment == AlignmentX.Left)
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
