using RAGE;
using RAGE.Game;
using System.Drawing;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.MapCreator
{
    class Draw
    {
        public static RGBA HighlightColor_Edge = new RGBA(255, 255, 255, 255);
        public static RGBA HighlightColor_Full = new RGBA(255, 255, 255, 35);

        public static void DrawSkeleton(Vector3 pos, Vector3 size, Vector3 rot)
        {
            Vector3 p1 = pos + new Vector3(size.X / 2, size.Y / 2, size.Z / 2);
            Vector3 p2 = pos + new Vector3(size.X / 2, -size.Y / 2, size.Z / 2);
            Vector3 p3 = pos + new Vector3(size.X / 2, -size.Y / 2, -size.Z / 2);
            Vector3 p4 = pos + new Vector3(size.X / 2, size.Y / 2, -size.Z / 2);

            Vector3 p5 = pos + new Vector3(-size.X / 2, size.Y / 2, size.Z / 2);
            Vector3 p6 = pos + new Vector3(-size.X / 2, -size.Y / 2, size.Z / 2);
            Vector3 p7 = pos + new Vector3(-size.X / 2, -size.Y / 2, -size.Z / 2);
            Vector3 p8 = pos + new Vector3(-size.X / 2, size.Y / 2, -size.Z / 2);

            p1 -= pos;
            p1 = ClientUtils.RotateY(p1, rot.Y);
            p1 = ClientUtils.RotateX(p1, rot.X);
            p1 = ClientUtils.RotateZ(p1, rot.Z);
            p1 += pos;

            p2 -= pos;
            p2 = ClientUtils.RotateY(p2, rot.Y);
            p2 = ClientUtils.RotateX(p2, rot.X);
            p2 = ClientUtils.RotateZ(p2, rot.Z);
            p2 += pos;

            p3 -= pos;
            p3 = ClientUtils.RotateY(p3, rot.Y);
            p3 = ClientUtils.RotateX(p3, rot.X);
            p3 = ClientUtils.RotateZ(p3, rot.Z);
            p3 += pos;

            p4 -= pos;
            p4 = ClientUtils.RotateY(p4, rot.Y);
            p4 = ClientUtils.RotateX(p4, rot.X);
            p4 = ClientUtils.RotateZ(p4, rot.Z);
            p4 += pos;

            p5 -= pos;
            p5 = ClientUtils.RotateY(p5, rot.Y);
            p5 = ClientUtils.RotateX(p5, rot.X);
            p5 = ClientUtils.RotateZ(p5, rot.Z);
            p5 += pos;

            p6 -= pos;
            p6 = ClientUtils.RotateY(p6, rot.Y);
            p6 = ClientUtils.RotateX(p6, rot.X);
            p6 = ClientUtils.RotateZ(p6, rot.Z);
            p6 += pos;

            p7 -= pos;
            p7 = ClientUtils.RotateY(p7, rot.Y);
            p7 = ClientUtils.RotateX(p7, rot.X);
            p7 = ClientUtils.RotateZ(p7, rot.Z);
            p7 += pos;

            p8 -= pos;
            p8 = ClientUtils.RotateY(p8, rot.Y);
            p8 = ClientUtils.RotateX(p8, rot.X);
            p8 = ClientUtils.RotateZ(p8, rot.Z);
            p8 += pos;
            Graphics.DrawLine(p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
            Graphics.DrawLine(p2.X, p2.Y, p2.Z, p3.X, p3.Y, p3.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
            Graphics.DrawLine(p3.X, p3.Y, p3.Z, p4.X, p4.Y, p4.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
            Graphics.DrawLine(p4.X, p4.Y, p4.Z, p1.X, p1.Y, p1.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);

            Graphics.DrawPoly(p3.X, p3.Y, p3.Z, p4.X, p4.Y, p4.Z, p1.X, p1.Y, p1.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
            Graphics.DrawPoly(p2.X, p2.Y, p2.Z, p3.X, p3.Y, p3.Z, p1.X, p1.Y, p1.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);

            Graphics.DrawLine(p5.X, p5.Y, p5.Z, p6.X, p6.Y, p6.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
            Graphics.DrawLine(p6.X, p6.Y, p6.Z, p7.X, p7.Y, p7.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
            Graphics.DrawLine(p7.X, p7.Y, p7.Z, p8.X, p8.Y, p8.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
            Graphics.DrawLine(p8.X, p8.Y, p8.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);

            Graphics.DrawPoly(p8.X, p8.Y, p8.Z, p7.X, p7.Y, p7.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
            Graphics.DrawPoly(p7.X, p7.Y, p7.Z, p6.X, p6.Y, p6.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);

            Graphics.DrawLine(p1.X, p1.Y, p1.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
            Graphics.DrawLine(p2.X, p2.Y, p2.Z, p6.X, p6.Y, p6.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
            Graphics.DrawLine(p3.X, p3.Y, p3.Z, p7.X, p7.Y, p7.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
            Graphics.DrawLine(p4.X, p4.Y, p4.Z, p8.X, p8.Y, p8.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);

            Graphics.DrawPoly(p1.X, p1.Y, p1.Z, p4.X, p4.Y, p4.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
            Graphics.DrawPoly(p5.X, p5.Y, p5.Z, p4.X, p4.Y, p4.Z, p8.X, p8.Y, p8.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);

            Graphics.DrawPoly(p2.X, p2.Y, p2.Z, p5.X, p5.Y, p5.Z, p6.X, p6.Y, p6.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
            Graphics.DrawPoly(p2.X, p2.Y, p2.Z, p1.X, p1.Y, p1.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);

            Graphics.DrawPoly(p3.X, p3.Y, p3.Z, p2.X, p2.Y, p2.Z, p6.X, p6.Y, p6.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
            Graphics.DrawPoly(p3.X, p3.Y, p3.Z, p6.X, p6.Y, p6.Z, p7.X, p7.Y, p7.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);

            Graphics.DrawPoly(p3.X, p3.Y, p3.Z, p7.X, p7.Y, p7.Z, p8.X, p8.Y, p8.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
            Graphics.DrawPoly(p8.X, p8.Y, p8.Z, p4.X, p4.Y, p4.Z, p3.X, p3.Y, p3.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
        }
    }
}
