﻿using System.Drawing;
using TDS.Client.Data.Enums;

namespace TDS.Client.Handler.Draw.Dx
{
    internal class DxLine : DxBase
    {
        //private readonly bool _relative;
        //private bool is3D;
        private readonly CamerasHandler _camerasHandler;

        private readonly Color _color;

        private readonly float _startX,
                            _startY,
            _startZ,
            _endX,
            _endY,
            _endZ;

        private readonly UtilsHandler _utilsHandler;

        public DxLine(DxHandler dxHandler, CamerasHandler camerasHandler, UtilsHandler utilsHandler, float startX, float startY, float? startZ, float endX, float endY, float? endZ,
            Color color, bool relative = true, int frontPriority = 0)
            : base(dxHandler, frontPriority: frontPriority)
        {
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;

            if (startZ.HasValue && endZ.HasValue)
            {
                _startX = startX;
                _startY = startY;
                _startZ = startZ.Value;
                _endX = endX;
                _endY = endY;
                _endZ = endZ.Value;
            }
            else
            {
                var worldStart = _utilsHandler.GetWorldCoordFromScreenCoord(GetRelativeX(startX, relative), GetRelativeY(startY, relative), _camerasHandler.ActiveCamera);
                this._startX = worldStart.X;
                this._startY = worldStart.Y;
                this._startZ = worldStart.Z;

                var worldEnd = _utilsHandler.GetWorldCoordFromScreenCoord(GetRelativeX(endX, relative), GetRelativeY(endY, relative), _camerasHandler.ActiveCamera);
                this._endX = worldEnd.X;
                this._endY = worldEnd.Y;
                this._endZ = worldEnd.Z;
            }

            this._color = color;
            //this._relative = relative;
        }

        public override void Draw()
        {
            RAGE.Game.Graphics.DrawLine(_startX, _startY, _startZ, _endX, _endY, _endZ, _color.R, _color.G, _color.B, _color.A);
        }

        public override DxType GetDxType()
        {
            return DxType.Line;
        }
    }
}
