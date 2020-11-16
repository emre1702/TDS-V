using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Models;
using TDS.Client.Handler.Entities;
using TDS.Client.Handler.Events;
using TDS.Client.Handler.Sync;
using TDS.Shared.Core;
using TDS.Shared.Data.Default;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Utility;
using static RAGE.Events;

namespace TDS.Client.Handler
{
    public class UtilsHandler : ServiceBase
    {
        private readonly DataSyncHandler _dataSyncHandler;

        public UtilsHandler(LoggingHandler loggingHandler, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler)
            : base(loggingHandler)
        {
            _dataSyncHandler = dataSyncHandler;

            eventsHandler.LoggedIn += EventsHandler_LoggedIn;

            Tick += DisableControlActions;
        }

        public Tuple<Vector3, Vector3> ClosestDistanceBetweenLines(Vector3 a0, Vector3 a1, Vector3 b0, Vector3 b1)
        {
            var A = a1 - a0;
            var B = b1 - b0;
            float magA = A.Length();
            float magB = B.Length();

            var _A = A / magA;
            var _B = B / magB;

            var cross = GetCrossProduct(_A, _B);
            var denom = cross.Length() * cross.Length();

            Vector3 closest1, closest2;
            if (denom == 0)
            {
                var d0 = GetDotProduct(_A, (b0 - a0));
                var d1 = GetDotProduct(_A, (b1 - a0));
                if (d0 <= 0 && 0 >= d1)
                {
                    if (MathF.Abs(d0) < MathF.Abs(d1))
                    {
                        closest1 = a0;
                        closest2 = b0;
                        return new Tuple<Vector3, Vector3>(closest1, closest2);
                    }
                    closest1 = a0;
                    closest2 = b1;
                    return new Tuple<Vector3, Vector3>(closest1, closest2);
                }
                else if (d0 >= magA && magA <= d1)
                {
                    if (MathF.Abs(d0) < MathF.Abs(d1))

                    {
                        closest1 = a1;
                        closest2 = b0;
                        return new Tuple<Vector3, Vector3>(closest1, closest2);
                    }
                    closest1 = a1;
                    closest2 = b1;
                    return new Tuple<Vector3, Vector3>(closest1, closest2);
                }

                return new Tuple<Vector3, Vector3>(new Vector3(), new Vector3());
            }

            var t = (b0 - a0);
            var detA = Determinent(t, _B, cross);
            var detB = Determinent(t, _A, cross);

            var t0 = detA / denom;
            var t1 = detB / denom;

            var pA = a0 + (_A * t0);
            var pB = b0 + (_B * t1);

            if (t0 < 0)
                pA = a0;
            else if (t0 > magA)
                pA = a1;

            if (t1 < 0)
                pB = b0;
            else if (t1 > magB)
                pB = b1;

            float dot;
            if (t0 < 0 || t0 > magA)
            {
                dot = GetDotProduct(_B, (pA - b0));
                if (dot < 0)
                    dot = 0;
                else if (dot > magB)
                    dot = magB;
                pB = b0 + (_B * dot);
            }

            if (t1 < 0 || t1 > magB)
            {
                dot = GetDotProduct(_A, (pB - a0));
                if (dot < 0)
                    dot = 0;
                else if (dot > magA)
                    dot = magA;
                pA = a0 + (_A * dot);
            }

            closest1 = pA;
            closest2 = pB;
            return new Tuple<Vector3, Vector3>(closest1, closest2);
        }

        public float DegreesToRad(float deg)
        {
            return MathF.PI * deg / 180.0f;
        }

        public float Determinent(Vector3 a, Vector3 b, Vector3 c)
        {
            return a.X * b.Y * c.Z + a.Y * b.Z * c.X + a.Z * b.X * c.Y - c.X * b.Y * a.Z - c.Y * b.Z * a.X - c.Z * b.X * a.Y;
        }

        public float GetAngleBetweenVectors(Vector3 v1, Vector3 v2)
        {
            return MathF.Acos(GetDotProduct(GetNormalizedVector(v1), GetNormalizedVector(v2)));
        }

        public Vector3 GetCrossProduct(Vector3 left, Vector3 right)
        {
            var vec = new Vector3
            {
                X = left.Y * right.Z - left.Z * right.Y,
                Y = left.Z * right.X - left.X * right.Z,
                Z = left.X * right.Y - left.Y * right.X
            };
            return vec;
        }

        public float GetCursorX()
        {
            return Pad.GetDisabledControlNormal((int)InputGroup.MOVE, (int)Control.CursorX);
        }

        public float GetCursorY()
        {
            return Pad.GetDisabledControlNormal((int)InputGroup.MOVE, (int)Control.CursorY);
        }

        public Vector3 GetDirectionByRotation(Vector3 rotation)
        {
            float num = rotation.Z * 0.0174532924f;
            float num2 = rotation.X * 0.0174532924f;
            float num3 = MathF.Abs(MathF.Cos(num2));
            return new Vector3 { X = -MathF.Sin(num) * num3, Y = MathF.Cos(num) * num3, Z = MathF.Sin(num2) };
        }

        public string GetDisplayName(ITDSPlayer player)
        {
            string name = player.Name;
            int adminLevel = Convert.ToInt32(_dataSyncHandler.GetData(player, PlayerDataKey.AdminLevel));
            if (adminLevel > SharedConstants.ServerTeamSuffixMinAdminLevel)
                name = SharedConstants.ServerTeamSuffix + name;
            return name;
        }

        public float GetDotProduct(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public Vector3 GetNormalizedVector(Vector3 vec)
        {
            float mag = MathF.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
            return new Vector3(vec.X / mag, vec.Y / mag, vec.Z / mag);
        }

        public ITDSPlayer GetPlayerByHandleValue(ushort handleValue)
        {
            return RAGE.Elements.Entities.Players.GetAtRemote(handleValue) as ITDSPlayer;
        }

        public Color GetRandomColor()
        {
            return Color.FromArgb(255, SharedUtils.Rnd.Next(255), SharedUtils.Rnd.Next(255), SharedUtils.Rnd.Next(255));
        }

        public Vector3 GetScreenCoordFromWorldCoord(Vector3 vec)
        {
            float x = 0;
            float y = 0;
            if (Graphics.GetScreenCoordFromWorldCoord(vec.X, vec.Y, vec.Z, ref x, ref y))
                return new Vector3(x, y, 0f);
            else
                return null;
        }

        public List<ITDSPlayer> GetTriggeredPlayersList(string objStr)
        {
            var list = Serializer.FromServer<List<ushort>>(objStr);
            var newList = new List<ITDSPlayer>();

            foreach (var handleValue in list)
            {
                var p = GetPlayerByHandleValue(handleValue);
                if (p is null)
                    continue;
                newList.Add(p);
            }

            return newList;
        }

        public Vector3 GetWorldCoordFromScreenCoord(float x, float y, TDSCamera tdsCamera = null)
        {
            var camPos = tdsCamera?.Position ?? Cam.GetGameplayCamCoord();
            var camRot = tdsCamera?.Rotation ?? Cam.GetGameplayCamRot(2);
            var camForward = RotationToDirection(camRot);
            var rotUp = camRot + new Vector3(1, 0, 0);
            var rotDown = camRot + new Vector3(-1, 0, 0);
            var rotLeft = camRot + new Vector3(0, 0, -1);
            var rotRight = camRot + new Vector3(0, 0, 1);

            var camRight = RotationToDirection(rotRight) - RotationToDirection(rotLeft);
            var camUp = RotationToDirection(rotUp) - RotationToDirection(rotDown);

            var rollRad = -DegreesToRad(camRot.Y);

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

        public bool LineIntersectingCircle(Vector3 CircleCenter, Vector3 CircleRotation, float CircleRadius, Vector3 LineStart, Vector3 LineEnd, ref Vector3 HitPosition, float threshold, ref Vector3 planeNorm)
        {
            Vector3 v2 = new Vector3(CircleCenter.X, CircleCenter.Y, CircleCenter.Z + CircleRadius);
            Vector3 v3 = new Vector3(CircleCenter.X - CircleRadius, CircleCenter.Y, CircleCenter.Z);

            v2 -= CircleCenter;
            v2 = RotateZ(v2, CircleRotation.Z);
            v2 += CircleCenter;

            v3 -= CircleCenter;
            v3 = RotateZ(v3, CircleRotation.Z);
            v3 += CircleCenter;

            v2 -= CircleCenter;
            v2 = RotateX(v2, CircleRotation.X);
            v2 += CircleCenter;

            v3 -= CircleCenter;
            v3 = RotateX(v3, CircleRotation.X);
            v3 += CircleCenter;

            v2 -= CircleCenter;
            v2 = RotateY(v2, CircleRotation.Y);
            v2 += CircleCenter;

            v3 -= CircleCenter;
            v3 = RotateY(v3, CircleRotation.Y);
            v3 += CircleCenter;

            //Graphics.DrawPoly(CircleCenter.X, CircleCenter.Y, CircleCenter.Z, v2.X, v2.Y, v2.Z, v3.X, v3.Y, v3.Z, 0, 255, 0, 255);

            Vector3 four = v2 - CircleCenter;
            Vector3 five = v3 - CircleCenter;

            Vector3 cross = GetCrossProduct(four, five);
            planeNorm = new Vector3(cross.X, cross.Y, cross.Z);
            cross.Normalize();
            bool hit = LineIntersectingPlane(cross, CircleCenter, LineStart, LineEnd, ref HitPosition);
            if (hit)
            {
                if (HitPosition.DistanceTo(CircleCenter) <= CircleRadius + threshold)
                {
                    return true;
                }
            }
            return false;
        }

        public bool LineIntersectingPlane(Vector3 PlaneNorm, Vector3 PlanePoint, Vector3 LineStart, Vector3 LineEnd, ref Vector3 HitPosition)
        {
            Vector3 u = LineEnd - LineStart;
            float dot = GetDotProduct(PlaneNorm, u);
            if (MathF.Abs(dot) > float.Epsilon)
            {
                Vector3 w = LineStart - PlanePoint;
                float fac = -GetDotProduct(PlaneNorm, w) / dot;
                u *= fac;
                HitPosition = LineStart + u;
                return true;
            }
            return false;
        }

        public bool LineIntersectingSphere(Vector3 StartLine, Vector3 LineEnd, Vector3 SphereCenter, float SphereRadius)
        {
            Vector3 d = LineEnd - StartLine;
            Vector3 f = StartLine - SphereCenter;

            float c = GetDotProduct(f, f) - SphereRadius * SphereRadius;
            if (c <= 0f)
                return true;

            float b = GetDotProduct(f, d);
            if (b >= 0f)
                return false;

            float a = GetDotProduct(d, d);
            if (b * b - a * c < 0f)
                return false;

            return true;
        }

        public void Notify(string msg)
        {
            Ui.SetNotificationTextEntry("STRING");
            Ui.AddTextComponentSubstringPlayerName(msg);
            Ui.DrawNotification(false, false);
        }

        public float RadToDegrees(float rad)
        {
            return rad * (180f / MathF.PI);
        }

        public RaycastHit RaycastFromTo(Vector3 from, Vector3 to, int ignoreEntity, int flags)
        {
            int ray = Shapetest.StartShapeTestRay(from.X, from.Y, from.Z, to.X, to.Y, to.Z, flags, ignoreEntity, 0);
            RaycastHit cast = new RaycastHit();
            int curtemp = 0;
            cast.ShapeResult = Shapetest.GetShapeTestResult(ray, ref curtemp, cast.EndCoords, cast.SurfaceNormal, ref cast.EntityHit);
            cast.Hit = Convert.ToBoolean(curtemp);
            return cast;
        }

        public Vector3 RotateX(Vector3 point, float angle)
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

        public Vector3 RotateY(Vector3 point, float angle)
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

        public Vector3 RotateZ(Vector3 point, float angle)
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

        public Vector3 RotationToDirection(Vector3 rotation)
        {
            var z = DegreesToRad(rotation.Z);
            var x = DegreesToRad(rotation.X);
            var num = Math.Abs(Math.Cos(x));
            return new Vector3
            {
                X = (float)(-Math.Sin(z) * num),
                Y = (float)(Math.Cos(z) * num),
                Z = (float)Math.Sin(x)
            };
        }

        private void DisableControlActions(List<TickNametagData> _)
        {
            Pad.DisableControlAction((int)InputGroup.WHEEL, (int)Control.EnterCheatCode, true);

            if (RAGE.Elements.Player.LocalPlayer.IsArmed((int)ArmedType.AllExceptMelee))
            {
                Pad.DisableControlAction((int)InputGroup.MOVE, (int)Control.MeleeAttackLight, true);
                Pad.DisableControlAction((int)InputGroup.MOVE, (int)Control.MeleeAttackHeavy, true);
                Pad.DisableControlAction((int)InputGroup.MOVE, (int)Control.MeleeAttackAlternate, true);
            }
        }

        private void EventsHandler_LoggedIn()
        {
            Tick += HideHudOriginalComponents;
        }

        private void HideHudOriginalComponents(List<TickNametagData> _)
        {
            Ui.HideHudComponentThisFrame((int)HudComponent.Cash);
            Ui.HideHudComponentThisFrame((int)HudComponent.WeaponWheelStats);
            Ui.HideHudComponentThisFrame((int)HudComponent.WeaponIcon);
            Ui.HideHudComponentThisFrame((int)HudComponent.CashChange);
            Ui.HideHudComponentThisFrame((int)HudComponent.MpCash);
        }
    }
}