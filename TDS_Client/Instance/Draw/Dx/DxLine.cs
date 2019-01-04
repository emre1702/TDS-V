using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TDS_Client.Enum;

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


        public DxLine(float startX, float startY, float? startZ, float endX, float endY, float? endZ, Color color, bool relative = true) : base()
        {
            this.startX = startX;
            this.startY = startY;
            this.startZ = startZ.HasValue ? startZ.Value : 0;
            this.endX = endX;
            this.endY = endY;
            this.endZ = endZ.HasValue ? endZ.Value : 0;
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
