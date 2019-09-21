using RAGE;
using RAGE.Game;
using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Instance.Draw.Dx
{
    internal class DxLine : Dx
    {
        private readonly float _startX,
            _startY,
            _startZ,
            _endX,
            _endY,
            _endZ;

        private readonly Color _color;
        //private readonly bool _relative;
        //private bool is3D;

        public DxLine(float startX, float startY, float? startZ, float endX, float endY, float? endZ, Color color, bool relative = true, int frontPriority = 0) : base(frontPriority: frontPriority)
        {
            if (startZ.HasValue && endZ.HasValue)
            {
                this._startX = startX;
                this._startY = startY;
                this._startZ = startZ.Value;
                this._endX = endX;
                this._endY = endY;
                this._endZ = endZ.Value;
            }
            else
            {
                Vector3 worldStart = ClientUtils.GetWorldCoordFromScreenCoord(GetRelativeX(startX, relative), GetRelativeY(startY, relative), TDSCamera.ActiveCamera);
                this._startX = worldStart.X;
                this._startY = worldStart.Y;
                this._startZ = worldStart.Z;

                Vector3 worldEnd = ClientUtils.GetWorldCoordFromScreenCoord(GetRelativeX(endX, relative), GetRelativeY(endY, relative), TDSCamera.ActiveCamera);
                this._endX = worldEnd.X;
                this._endY = worldEnd.Y;
                this._endZ = worldEnd.Z;
            }

            this._color = color;
            //this._relative = relative;
        }

        public override void Draw()
        {
            Graphics.DrawLine(_startX, _startY, _startZ, _endX, _endY, _endZ, _color.R, _color.G, _color.B, _color.A);
        }

        public override EDxType GetDxType()
        {
            return EDxType.Line;
        }
    }
}