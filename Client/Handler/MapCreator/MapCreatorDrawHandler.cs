﻿using System.Drawing;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorDrawHandler
    {
        #region Public Fields

        public Color HighlightColor_Edge = Color.FromArgb(255, 255, 255, 255);
        public Color HighlightColor_Full = Color.FromArgb(35, 255, 255, 255);

        #endregion Public Fields

        #region Private Fields

        private readonly UtilsHandler _utilsHandler;
        private readonly IModAPI ModAPI;

        #endregion Private Fields

        #region Public Constructors

        public MapCreatorDrawHandler(IModAPI modAPI, UtilsHandler utilsHandler)
        {
            ModAPI = modAPI;
            _utilsHandler = utilsHandler;
        }

        #endregion Public Constructors

        #region Public Methods

        public void DrawSkeleton(Position pos, Position size, Position rot)
        {
            Position p1 = pos + new Position(size.X / 2, size.Y / 2, size.Z / 2);
            Position p2 = pos + new Position(size.X / 2, -size.Y / 2, size.Z / 2);
            Position p3 = pos + new Position(size.X / 2, -size.Y / 2, -size.Z / 2);
            Position p4 = pos + new Position(size.X / 2, size.Y / 2, -size.Z / 2);

            Position p5 = pos + new Position(-size.X / 2, size.Y / 2, size.Z / 2);
            Position p6 = pos + new Position(-size.X / 2, -size.Y / 2, size.Z / 2);
            Position p7 = pos + new Position(-size.X / 2, -size.Y / 2, -size.Z / 2);
            Position p8 = pos + new Position(-size.X / 2, size.Y / 2, -size.Z / 2);

            p1 -= pos;
            p1 = _utilsHandler.RotateY(p1, rot.Y);
            p1 = _utilsHandler.RotateX(p1, rot.X);
            p1 = _utilsHandler.RotateZ(p1, rot.Z);
            p1 += pos;

            p2 -= pos;
            p2 = _utilsHandler.RotateY(p2, rot.Y);
            p2 = _utilsHandler.RotateX(p2, rot.X);
            p2 = _utilsHandler.RotateZ(p2, rot.Z);
            p2 += pos;

            p3 -= pos;
            p3 = _utilsHandler.RotateY(p3, rot.Y);
            p3 = _utilsHandler.RotateX(p3, rot.X);
            p3 = _utilsHandler.RotateZ(p3, rot.Z);
            p3 += pos;

            p4 -= pos;
            p4 = _utilsHandler.RotateY(p4, rot.Y);
            p4 = _utilsHandler.RotateX(p4, rot.X);
            p4 = _utilsHandler.RotateZ(p4, rot.Z);
            p4 += pos;

            p5 -= pos;
            p5 = _utilsHandler.RotateY(p5, rot.Y);
            p5 = _utilsHandler.RotateX(p5, rot.X);
            p5 = _utilsHandler.RotateZ(p5, rot.Z);
            p5 += pos;

            p6 -= pos;
            p6 = _utilsHandler.RotateY(p6, rot.Y);
            p6 = _utilsHandler.RotateX(p6, rot.X);
            p6 = _utilsHandler.RotateZ(p6, rot.Z);
            p6 += pos;

            p7 -= pos;
            p7 = _utilsHandler.RotateY(p7, rot.Y);
            p7 = _utilsHandler.RotateX(p7, rot.X);
            p7 = _utilsHandler.RotateZ(p7, rot.Z);
            p7 += pos;

            p8 -= pos;
            p8 = _utilsHandler.RotateY(p8, rot.Y);
            p8 = _utilsHandler.RotateX(p8, rot.X);
            p8 = _utilsHandler.RotateZ(p8, rot.Z);
            p8 += pos;

            ModAPI.Graphics.DrawLine(p1, p2, HighlightColor_Edge);
            ModAPI.Graphics.DrawLine(p2, p3, HighlightColor_Edge);
            ModAPI.Graphics.DrawLine(p3, p4, HighlightColor_Edge);
            ModAPI.Graphics.DrawLine(p4, p1, HighlightColor_Edge);

            ModAPI.Graphics.DrawPoly(p3, p4, p1, HighlightColor_Full);
            ModAPI.Graphics.DrawPoly(p2, p3, p1, HighlightColor_Full);

            ModAPI.Graphics.DrawLine(p5, p6, HighlightColor_Edge);
            ModAPI.Graphics.DrawLine(p6, p7, HighlightColor_Edge);
            ModAPI.Graphics.DrawLine(p7, p8, HighlightColor_Edge);
            ModAPI.Graphics.DrawLine(p8, p5, HighlightColor_Edge);

            ModAPI.Graphics.DrawPoly(p8, p7, p5, HighlightColor_Full);
            ModAPI.Graphics.DrawPoly(p7, p6, p5, HighlightColor_Full);

            ModAPI.Graphics.DrawLine(p1, p5, HighlightColor_Edge);
            ModAPI.Graphics.DrawLine(p2, p6, HighlightColor_Edge);
            ModAPI.Graphics.DrawLine(p3, p7, HighlightColor_Edge);
            ModAPI.Graphics.DrawLine(p4, p8, HighlightColor_Edge);

            ModAPI.Graphics.DrawPoly(p1, p4, p5, HighlightColor_Full);
            ModAPI.Graphics.DrawPoly(p5, p4, p8, HighlightColor_Full);

            ModAPI.Graphics.DrawPoly(p2, p5, p6, HighlightColor_Full);
            ModAPI.Graphics.DrawPoly(p2, p1, p5, HighlightColor_Full);

            ModAPI.Graphics.DrawPoly(p3, p2, p6, HighlightColor_Full);
            ModAPI.Graphics.DrawPoly(p3, p6, p7, HighlightColor_Full);

            ModAPI.Graphics.DrawPoly(p3, p7, p8, HighlightColor_Full);
            ModAPI.Graphics.DrawPoly(p8, p4, p3, HighlightColor_Full);
        }

        #endregion Public Methods
    }
}
