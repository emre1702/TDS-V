using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Utility;

namespace TDS_Client.Handler
{
    public class UtilsHandler : ServiceBase
    {
        #region Private Fields

        private readonly DataSyncHandler _dataSyncHandler;
        private readonly Serializer _serializer;

        #endregion Private Fields

        #region Public Constructors

        public UtilsHandler(IModAPI modAPI, LoggingHandler loggingHandler, Serializer serializer, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            _serializer = serializer;
            _dataSyncHandler = dataSyncHandler;

            eventsHandler.LoggedIn += EventsHandler_LoggedIn;

            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(DisableControlActions));
        }

        #endregion Public Constructors

        #region Public Methods

        public Tuple<Position3D, Position3D> ClosestDistanceBetweenLines(Position3D a0, Position3D a1, Position3D b0, Position3D b1)
        {
            var A = a1 - a0;
            var B = b1 - b0;
            float magA = A.Length();
            float magB = B.Length();

            var _A = A / magA;
            var _B = B / magB;

            var cross = GetCrossProduct(_A, _B);
            var denom = cross.Length() * cross.Length();

            Position3D closest1, closest2;
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
                        return new Tuple<Position3D, Position3D>(closest1, closest2);
                    }
                    closest1 = a0;
                    closest2 = b1;
                    return new Tuple<Position3D, Position3D>(closest1, closest2);
                }
                else if (d0 >= magA && magA <= d1)
                {
                    if (MathF.Abs(d0) < MathF.Abs(d1))

                    {
                        closest1 = a1;
                        closest2 = b0;
                        return new Tuple<Position3D, Position3D>(closest1, closest2);
                    }
                    closest1 = a1;
                    closest2 = b1;
                    return new Tuple<Position3D, Position3D>(closest1, closest2);
                }

                return new Tuple<Position3D, Position3D>(new Position3D(), new Position3D());
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
            return new Tuple<Position3D, Position3D>(closest1, closest2);
        }

        public float DegreesToRad(float deg)
        {
            return MathF.PI * deg / 180.0f;
        }

        public float Determinent(Position3D a, Position3D b, Position3D c)
        {
            return a.X * b.Y * c.Z + a.Y * b.Z * c.X + a.Z * b.X * c.Y - c.X * b.Y * a.Z - c.Y * b.Z * a.X - c.Z * b.X * a.Y;
        }

        public float GetAngleBetweenVectors(Position3D v1, Position3D v2)
        {
            return MathF.Acos(GetDotProduct(GetNormalizedVector(v1), GetNormalizedVector(v2)));
        }

        public Position3D GetCrossProduct(Position3D left, Position3D right)
        {
            Position3D vec = new Position3D
            {
                X = left.Y * right.Z - left.Z * right.Y,
                Y = left.Z * right.X - left.X * right.Z,
                Z = left.X * right.Y - left.Y * right.X
            };
            return vec;
        }

        public float GetCursorX()
        {
            return ModAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.CursorX);
        }

        public float GetCursorY()
        {
            return ModAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.CursorY);
        }

        public Position3D GetDirectionByRotation(Position3D rotation)
        {
            float num = rotation.Z * 0.0174532924f;
            float num2 = rotation.X * 0.0174532924f;
            float num3 = MathF.Abs(MathF.Cos(num2));
            return new Position3D { X = -MathF.Sin(num) * num3, Y = MathF.Cos(num) * num3, Z = MathF.Sin(num2) };
        }

        public string GetDisplayName(IPlayer player)
        {
            string name = player.Name;
            int adminLevel = Convert.ToInt32(_dataSyncHandler.GetData(player, PlayerDataKey.AdminLevel));
            if (adminLevel > SharedConstants.ServerTeamSuffixMinAdminLevel)
                name = SharedConstants.ServerTeamSuffix + name;
            return name;
        }

        public float GetDotProduct(Position3D v1, Position3D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public Position3D GetNormalizedVector(Position3D vec)
        {
            float mag = MathF.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
            return new Position3D(vec.X / mag, vec.Y / mag, vec.Z / mag);
        }

        public IPlayer GetPlayerByHandleValue(ushort handleValue)
        {
            return ModAPI.Pool.Players.GetAtRemote(handleValue);
        }

        public Color GetRandomColor()
        {
            return Color.FromArgb(255, SharedUtils.Rnd.Next(255), SharedUtils.Rnd.Next(255), SharedUtils.Rnd.Next(255));
        }

        public Position3D GetScreenCoordFromWorldCoord(Position3D vec)
        {
            float x = 0;
            float y = 0;
            if (ModAPI.Graphics.GetScreenCoordFromWorldCoord(vec.X, vec.Y, vec.Z, ref x, ref y))
                return new Position3D(x, y, 0f);
            else
                return null;
        }

        public List<IPlayer> GetTriggeredPlayersList(string objStr)
        {
            var list = _serializer.FromServer<List<ushort>>(objStr);
            var newList = new List<IPlayer>();

            foreach (var handleValue in list)
            {
                var p = GetPlayerByHandleValue(handleValue);
                if (p is null)
                    continue;
                newList.Add(p);
            }

            return newList;
        }

        public Position3D GetWorldCoordFromScreenCoord(float x, float y, TDSCamera tdsCamera = null)
        {
            Position3D camPos = tdsCamera?.Position ?? ModAPI.Cam.GetGameplayCamCoord();
            Position3D camRot = tdsCamera?.Rotation ?? ModAPI.Cam.GetGameplayCamRot();
            var camForward = RotationToDirection(camRot);
            var rotUp = camRot + new Position3D(1, 0, 0);
            var rotDown = camRot + new Position3D(-1, 0, 0);
            var rotLeft = camRot + new Position3D(0, 0, -1);
            var rotRight = camRot + new Position3D(0, 0, 1);

            var camRight = RotationToDirection(rotRight) - RotationToDirection(rotLeft);
            var camUp = RotationToDirection(rotUp) - RotationToDirection(rotDown);

            var rollRad = -DegreesToRad(camRot.Y);

            var camRightRoll = camRight * (float)Math.Cos(rollRad) - camUp * (float)Math.Sin(rollRad);
            var camUpRoll = camRight * (float)Math.Sin(rollRad) + camUp * (float)Math.Cos(rollRad);

            var point3D = camPos + camForward * 1.0f + camRightRoll + camUpRoll;
            float point2DX = 0, point2DY = 0;
            if (!ModAPI.Graphics.GetScreenCoordFromWorldCoord(point3D.X, point3D.Y, point3D.Z, ref point2DX, ref point2DY))
            {
                //forwardDirection = camForward;
                return camPos + camForward * 1.0f;
            }

            var point3DZero = camPos + camForward * 1.0f;
            float point2DZeroX = 0, point2DZeroY = 0;
            if (!ModAPI.Graphics.GetScreenCoordFromWorldCoord(point3DZero.X, point3DZero.Y, point3DZero.Z, ref point2DZeroX, ref point2DZeroY))
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

        public bool LineIntersectingCircle(Position3D CircleCenter, Position3D CircleRotation, float CircleRadius, Position3D LineStart, Position3D LineEnd, ref Position3D HitPosition, float threshold, ref Position3D planeNorm)
        {
            Position3D v2 = new Position3D(CircleCenter.X, CircleCenter.Y, CircleCenter.Z + CircleRadius);
            Position3D v3 = new Position3D(CircleCenter.X - CircleRadius, CircleCenter.Y, CircleCenter.Z);

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

            //RAGE.Game.Graphics.DrawPoly(CircleCenter.X, CircleCenter.Y, CircleCenter.Z, v2.X, v2.Y, v2.Z, v3.X, v3.Y, v3.Z, 0, 255, 0, 255);

            Position3D four = v2 - CircleCenter;
            Position3D five = v3 - CircleCenter;

            Position3D cross = GetCrossProduct(four, five);
            planeNorm = new Position3D(cross.X, cross.Y, cross.Z);
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

        public bool LineIntersectingPlane(Position3D PlaneNorm, Position3D PlanePoint, Position3D LineStart, Position3D LineEnd, ref Position3D HitPosition)
        {
            Position3D u = LineEnd - LineStart;
            float dot = GetDotProduct(PlaneNorm, u);
            if (MathF.Abs(dot) > float.Epsilon)
            {
                Position3D w = LineStart - PlanePoint;
                float fac = -GetDotProduct(PlaneNorm, w) / dot;
                u *= fac;
                HitPosition = LineStart + u;
                return true;
            }
            return false;
        }

        public bool LineIntersectingSphere(Position3D StartLine, Position3D LineEnd, Position3D SphereCenter, float SphereRadius)
        {
            Position3D d = LineEnd - StartLine;
            Position3D f = StartLine - SphereCenter;

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
            ModAPI.Ui.SetNotificationTextEntry("STRING");
            ModAPI.Ui.AddTextComponentSubstringPlayerName(msg);
            ModAPI.Ui.DrawNotification(false);
        }

        public float RadToDegrees(float rad)
        {
            return rad * (180f / MathF.PI);
        }

        public RaycastHit RaycastFromTo(Position3D from, Position3D to, int ignoreEntity, int flags)
        {
            int ray = ModAPI.Shapetest.StartShapeTestRay(from.X, from.Y, from.Z, to.X, to.Y, to.Z, flags, ignoreEntity, 0);
            RaycastHit cast = new RaycastHit();
            int curtemp = 0;
            cast.ShapeResult = ModAPI.Shapetest.GetShapeTestResult(ray, ref curtemp, cast.EndCoords, cast.SurfaceNormal, ref cast.EntityHit);
            cast.Hit = Convert.ToBoolean(curtemp);
            return cast;
        }

        public Position3D RotateX(Position3D point, float angle)
        {
            Position3D f1 = new Position3D(1, 0, 0);
            Position3D f2 = new Position3D(0, MathF.Cos(DegreesToRad(angle)), -MathF.Sin(DegreesToRad(angle)));
            Position3D f3 = new Position3D(0, MathF.Sin(DegreesToRad(angle)), MathF.Cos(DegreesToRad(angle)));

            Position3D final = new Position3D
            {
                X = (f1.X * point.X + f1.Y * point.Y + f1.Z * point.Z),
                Y = (f2.X * point.X + f2.Y * point.Y + f2.Z * point.Z),
                Z = (f3.X * point.X + f3.Y * point.Y + f3.Z * point.Z)
            };

            return final;
        }

        public Position3D RotateY(Position3D point, float angle)
        {
            Position3D f4 = new Position3D(MathF.Cos(DegreesToRad(angle)), 0, MathF.Sin(DegreesToRad(angle)));
            Position3D f5 = new Position3D(0, 1, 0);
            Position3D f6 = new Position3D(-MathF.Sin(DegreesToRad(angle)), 0, MathF.Cos(DegreesToRad(angle)));

            Position3D final = new Position3D
            {
                X = (f4.X * point.X + f4.Y * point.Y + f4.Z * point.Z),
                Y = (f5.X * point.X + f5.Y * point.Y + f5.Z * point.Z),
                Z = (f6.X * point.X + f6.Y * point.Y + f6.Z * point.Z)
            };
            return final;
        }

        public Position3D RotateZ(Position3D point, float angle)
        {
            Position3D f7 = new Position3D(MathF.Cos(DegreesToRad(angle)), -MathF.Sin(DegreesToRad(angle)), 0);
            Position3D f8 = new Position3D(MathF.Sin(DegreesToRad(angle)), MathF.Cos(DegreesToRad(angle)), 0);
            Position3D f9 = new Position3D(0, 0, 1);

            Position3D final = new Position3D
            {
                X = (f7.X * point.X + f7.Y * point.Y + f7.Z * point.Z),
                Y = (f8.X * point.X + f8.Y * point.Y + f8.Z * point.Z),
                Z = (f9.X * point.X + f9.Y * point.Y + f9.Z * point.Z)
            };
            return final;
        }

        public Position3D RotationToDirection(Position3D rotation)
        {
            var z = DegreesToRad(rotation.Z);
            var x = DegreesToRad(rotation.X);
            var num = Math.Abs(Math.Cos(x));
            return new Position3D
            {
                X = (float)(-Math.Sin(z) * num),
                Y = (float)(Math.Cos(z) * num),
                Z = (float)Math.Sin(x)
            };
        }

        #endregion Public Methods

        #region Private Methods

        private void DisableControlActions(int _)
        {
            ModAPI.Control.DisableControlAction(InputGroup.WHEEL, Control.EnterCheatCode);
        }

        private void EventsHandler_LoggedIn()
        {
            ModAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(HideHudOriginalComponents));
        }

        private void HideHudOriginalComponents(int _)
        {
            ModAPI.Ui.HideHudComponentThisFrame(HudComponent.HUD_CASH);
            ModAPI.Ui.HideHudComponentThisFrame(HudComponent.HUD_WEAPON_WHEEL_STATS);
            ModAPI.Ui.HideHudComponentThisFrame(HudComponent.HUD_WEAPON_ICON);
        }

        #endregion Private Methods
    }
}
