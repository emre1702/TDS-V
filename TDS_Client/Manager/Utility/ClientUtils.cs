﻿using Newtonsoft.Json;
using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Lobby;
using TDS_Common.Manager.Utility;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    internal static class ClientUtils
    {
        static ClientUtils()
        {
            TickManager.Add(DisableAttack, () => Bomb.BombOnHand || !Round.InFight);
        }

        public static List<Player> GetTriggeredPlayersList(string objStr)
        {
            var list = JsonConvert.DeserializeObject<List<ushort>>(objStr);
            return list.Select(s => GetPlayerByHandleValue(s)).ToList();
        }

        public static Player GetPlayerByHandleValue(ushort handleValue)
        {
            return Entities.Players.GetAtRemote(handleValue);
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

        public static Vector3 GetWorldCoordFromScreenCoord(float x, float y, TDSCamera cam = null /*, out Vector3 forwardDirection*/)
        {
            Vector3 camPos = cam?.Position ?? Cam.GetGameplayCamCoord();
            Vector3 camRot = cam?.Rotation ?? Cam.GetGameplayCamRot(0);
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

        public static Color GetContrast(this Color original)
        {
            var l = 0.2126 * (original.R / 255d) + 0.7152 * (original.G / 255d) + 0.0722 * (original.B / 255d);

            return l < 0.4 ? Color.White : Color.Black;
        }

        public static Vector3 GetDirectionByRotation(Vector3 rotation)
        {
            float num = rotation.Z * 0.0174532924f;
            float num2 = rotation.X * 0.0174532924f;
            float num3 = MathF.Abs(MathF.Cos(num2));
            return new Vector3 { X = -MathF.Sin(num) * num3, Y = MathF.Cos(num) * num3, Z = MathF.Sin(num2) };
        }

        public static Vector3 GetCrossProduct(Vector3 left, Vector3 right)
        {
            Vector3 vec = new Vector3
            {
                X = left.Y * right.Z - left.Z * right.Y,
                Y = left.Z * right.X - left.X * right.Z,
                Z = left.X * right.Y - left.Y * right.X
            };
            return vec;
        }

        public static Vector3 RotateX(Vector3 point, float angle)
        {
            Vector3 f1 = new Vector3(1, 0, 0);
            Vector3 f2 = new Vector3(0, MathF.Cos(DegreesToRad(angle)), -MathF.Sin(DegreesToRad(angle)));
            Vector3 f3 = new Vector3(0, MathF.Sin(DegreesToRad(angle)), MathF.Cos(DegreesToRad(angle)));

            Vector3 final = new Vector3
            {
                X = (f1.X * point.X + f1.Y * point.Y + f1.Z * point.Z),
                Y = (f2.X * point.X + f2.Y * point.Y + f2.Z * point.Z),
                Z = (f3.X * point.X + f3.Y * point.Y + f3.Z * point.Z)
            };

            return final;
        }

        public static Vector3 RotateZ(Vector3 point, float angle)
        {
            Vector3 f7 = new Vector3(MathF.Cos(DegreesToRad(angle)), -MathF.Sin(DegreesToRad(angle)), 0);
            Vector3 f8 = new Vector3(MathF.Sin(DegreesToRad(angle)), MathF.Cos(DegreesToRad(angle)), 0);
            Vector3 f9 = new Vector3(0, 0, 1);

            Vector3 final = new Vector3
            {
                X = (f7.X * point.X + f7.Y * point.Y + f7.Z * point.Z),
                Y = (f8.X * point.X + f8.Y * point.Y + f8.Z * point.Z),
                Z = (f9.X * point.X + f9.Y * point.Y + f9.Z * point.Z)
            };
            return final;
        }

        public static Vector3 RotateY(Vector3 point, float angle)
        {
            Vector3 f4 = new Vector3(MathF.Cos(DegreesToRad(angle)), 0, MathF.Sin(DegreesToRad(angle)));
            Vector3 f5 = new Vector3(0, 1, 0);
            Vector3 f6 = new Vector3(-MathF.Sin(DegreesToRad(angle)), 0, MathF.Cos(DegreesToRad(angle)));

            Vector3 final = new Vector3
            {
                X = (f4.X * point.X + f4.Y * point.Y + f4.Z * point.Z),
                Y = (f5.X * point.X + f5.Y * point.Y + f5.Z * point.Z),
                Z = (f6.X * point.X + f6.Y * point.Y + f6.Z * point.Z)
            };
            return final;
        }

        public static float DegreesToRad(float deg)
        {
            return MathF.PI * deg / 180.0f;
        }
    }
}