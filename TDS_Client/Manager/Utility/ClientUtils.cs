using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Common.Manager.Utility;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    static class ClientUtils
    {
        public static List<Player> GetTriggeredPlayersList(object argobj)
        {
            List<ushort> arg = ((JArray)argobj).ToObject<List<ushort>>();
            return arg.Select(s => Entities.Players.GetAtRemote(s)).ToList();
        }

        public static void DisableAttack()
        {
            Pad.DisableControlAction(1, 24, true);
            Pad.DisableControlAction(1, 140, true);
            Pad.DisableControlAction(1, 141, true);
            Pad.DisableControlAction(1, 142, true);
            Pad.DisableControlAction(1, 257, true);
            Pad.DisableControlAction(1, 263, true);
            Pad.DisableControlAction(1, 264, true);
        }

        public static void Notify(string msg)
        {
            Ui.SetNotificationTextEntry("STRING");
            Ui.AddTextComponentSubstringPlayerName(msg);
            Ui.DrawNotification(false, false);
        }

        public static Vector3 GetWorldCoordFromScreenCoord(float x, float y /*, out Vector3 forwardDirection*/)
        {
            Vector3 camPos = Cam.GetGameplayCamCoord();
            Vector3 camRot = Cam.GetGameplayCamRot(0);
            var camForward = RotationToDirection(camRot);
            var rotUp = camRot + new Vector3(1, 0, 0);
            var rotDown = camRot + new Vector3(-1, 0, 0);
            var rotLeft = camRot + new Vector3(0, 0, -1);
            var rotRight = camRot + new Vector3(0, 0, 1);

            var camRight = RotationToDirection(rotRight) - RotationToDirection(rotLeft);
            var camUp = RotationToDirection(rotUp) - RotationToDirection(rotDown);

            var rollRad = -DegToRad(camRot.Y);

            var camRightRoll = camRight * (float)Math.Cos(rollRad) - camUp * (float)Math.Sin(rollRad);
            var camUpRoll = camRight * (float)Math.Sin(rollRad) + camUp * (float)Math.Cos(rollRad);

            var point3D = camPos + camForward * 1.0f + camRightRoll + camUpRoll;
            float point2DX = 0, point2DY = 0;
            if (!Graphics.GetScreenCoordFromWorldCoord(point3D.X, point3D.Y, point3D.Z, ref point2DX, ref point2DY))
            {
                //forwardDirection = camForward;
                return camPos + camForward * 1.0f;
            }

            var point3DZero = camPos + camForward * 1.0f;
            float point2DZeroX = 0, point2DZeroY = 0;
            if (!Graphics.GetScreenCoordFromWorldCoord(point3DZero.X, point3DZero.Y, point3DZero.Z, ref point2DZeroX, ref point2DZeroY))
            {
                //forwardDirection = camForward;
                return camPos + camForward * 1.0f;
            }

            const double eps = 0.001;
            if (Math.Abs(point2DX - point2DZeroX) < eps || Math.Abs(point2DY - point2DZeroY) < eps)
            {
                //forwardDirection = camForward;
                return camPos + camForward * 1.0f;
            }
            var scaleX = (x - point2DZeroX) / (point2DX - point2DZeroX);
            var scaleY = (y - point2DZeroY) / (point2DY - point2DZeroY);
            var point3Dret = camPos + camForward * 1.0f + camRightRoll * scaleX + camUpRoll * scaleY;
            //forwardDirection = camForward + camRightRoll * scaleX + camUpRoll * scaleY;
            return point3Dret;
        }

        public static float DegToRad(float _deg)
        {
            double Radian = (Math.PI / 180) * _deg;
            return (float)Radian;
        }

        public static Vector3 RotationToDirection(Vector3 rotation)
        {
            var z = DegToRad(rotation.Z);
            var x = DegToRad(rotation.X);
            var num = Math.Abs(Math.Cos(x));
            return new Vector3
            {
                X = (float)(-Math.Sin(z) * num),
                Y = (float)(Math.Cos(z) * num),
                Z = (float)Math.Sin(x)
            };
        }

        public static Color GetRandomColor()
        {
            return Color.FromArgb(255, CommonUtils.Rnd.Next(255), CommonUtils.Rnd.Next(255), CommonUtils.Rnd.Next(255));
        }
    }
}
