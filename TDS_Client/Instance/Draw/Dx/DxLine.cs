using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TDS_Client.Enum;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Instance.Draw.Dx
{
    class DxLine : Dx
    {
        private float startX,
            startY,
            startZ,
            endX,
            endY,
            endZ;
        private Color color;
        private bool relative;
        //private bool is3D;


        public DxLine(float startX, float startY, float? startZ, float endX, float endY, float? endZ, Color color, bool relative = true) : base()
        {
            if (startZ.HasValue && endZ.HasValue)
            {
                this.startX = startX;
                this.startY = startY;
                this.startZ = startZ.Value;
                this.endX = endX;
                this.endY = endY;
                this.endZ = endZ.Value;
            } 
            else
            {
                Vector3 worldStart = ClientUtils.GetWorldCoordFromScreenCoord(GetRelativeX(startX, relative), GetRelativeY(startY, relative));
                this.startX = worldStart.X;
                this.startY = worldStart.Y;
                this.startZ = worldStart.Z;

                Vector3 worldEnd = ClientUtils.GetWorldCoordFromScreenCoord(GetRelativeX(endX, relative), GetRelativeY(endY, relative));
                this.endX = worldEnd.X;
                this.endY = worldEnd.Y;
                this.endZ = worldEnd.Z;
            }
            
            this.color = color;
            this.relative = relative;
        }

        public override void Draw()
        {
            Graphics.DrawLine(startX, startY, startZ, endX, endY, endZ, color.R, color.G, color.B, color.A);
        }

        public override EDxType GetDxType()
        {
            return EDxType.Line;
        }
    }
}
