﻿using RAGE;
using RAGE.Game;
using RAGE.NUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Instance.Utility;
using TDS_Server.Dto.Map;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Instance.Lobby
{
    internal class MapLimit
    {
        private float _minX, _minY, _maxX, _maxY;
        private List<Position4DDto> _edges;
        private int _maxOutsideCounter;
        private float _edgesMaxTop = -1;

        private int _outsideCounter;
        private DxText _info;
        private TDSTimer _checkTimer;
        private TDSTimer _checkTimerFaster;
        private Vector3 _lastPosInMap;
        private float _lastRotInMap;
        private bool _createdGpsRoutes;
        private bool _started;

        private readonly EMapLimitType _type;
        private readonly Dictionary<EMapLimitType, Action> _mapLimitTypeMethod = new Dictionary<EMapLimitType, Action>{};
        private readonly HashSet<EMapLimitType> _typeToCheckFaster = new HashSet<EMapLimitType> { EMapLimitType.Block }; 

        private bool _savePosition => _edges != null && (_type == EMapLimitType.Block || _type == EMapLimitType.TeleportBackAfterTime);

        public MapLimit(List<Position4DDto> edges, EMapLimitType type)
        {
            _type = type;

            SetEdges(edges);            

            _mapLimitTypeMethod[EMapLimitType.KillAfterTime] = IsOutsideKillAfterTime;
            _mapLimitTypeMethod[EMapLimitType.TeleportBackAfterTime] = IsOutsideTeleportBackAfterTime;
            _mapLimitTypeMethod[EMapLimitType.Block] = IsOutsideBlock;
        }

        public void Start()
        {
            if (_started)
                Stop();
            Reset();
            if (_type != EMapLimitType.Display)
            {
                _checkTimer = new TDSTimer(Check, 1000, 0);
                _checkTimerFaster = new TDSTimer(CheckFaster, ClientConstants.MapLimitFasterCheckTimeMs, 0);
            }
            TickManager.Add(Draw);
            DrawGpsRoutes();
            _started = true;
        }

        public void Stop()
        {
            if (!_started)
                return;
            _checkTimer?.Kill();
            _checkTimer = null;
            _checkTimerFaster?.Kill();
            _checkTimerFaster = null;
            _info?.Remove();
            _info = null;
            _maxOutsideCounter = Settings.MapLimitTime;
            _outsideCounter = _maxOutsideCounter;
            TickManager.Remove(Draw);
            ClearGpsRoutes();
            _started = false;
        }

        private void Reset()
        {
            if (_savePosition)
            {
                _lastPosInMap = Player.LocalPlayer.Position;
                _lastRotInMap = Player.LocalPlayer.GetHeading();
            }
            _maxOutsideCounter = Settings.MapLimitTime;
            if (_outsideCounter == _maxOutsideCounter)
                return;
            _info?.Remove();
            _info = null;
            _outsideCounter = _maxOutsideCounter;
        }

        public void SetEdges(List<Position4DDto> edges)
        {
            if (_type != EMapLimitType.Display)
            {
                _minX = edges.Count > 0 ? edges.Min(v => v.X) : 0;
                _minY = edges.Count > 0 ? edges.Min(v => v.Y) : 0;
                _maxX = edges.Count > 0 ? edges.Max(v => v.X) : 0;
                _maxY = edges.Count > 0 ? edges.Max(v => v.Y) : 0;
            }

            foreach (var edge in edges)
            {
                float edgeZ = 0;
                if (Misc.GetGroundZFor3dCoord(edge.X, edge.Y, edge.Z + 1, ref edgeZ, false))
                    edge.Z = edgeZ;
            }

            _edges = edges;

            if (_started)
            {
                ClearGpsRoutes();
                DrawGpsRoutes();
            }
        }

        public void CheckFaster()
        {
            if (!_typeToCheckFaster.Contains(_type))
                return;

            if (_edges == null || IsWithin())
            {
                Reset();
                return;
            }

            if (_mapLimitTypeMethod.ContainsKey(_type))
                _mapLimitTypeMethod[_type]();
        }

        private void Check()
        {
            if (_typeToCheckFaster.Contains(_type))
                return;

            if (_edges == null || IsWithin())
            {
                Reset();
                return;
            }
            --_outsideCounter;

            if (_mapLimitTypeMethod.ContainsKey(_type))
                _mapLimitTypeMethod[_type]();
        }

        private void IsOutsideKillAfterTime()
        {
            if (_outsideCounter > 0)
                RefreshInfoKillAfterTime();
            else
            {
                EventsSender.Send(DToServerEvent.OutsideMapLimit);
                Stop();
            }
        }

        private void IsOutsideTeleportBackAfterTime()
        {
            if (_outsideCounter > 0)
                RefreshInfoTeleportAfterTime();
            else
            {
                Player.LocalPlayer.Position = _lastPosInMap;
                Reset();
            }    
        }

        private void IsOutsideBlock()
        {
            Player.LocalPlayer.Position = _lastPosInMap;
            Player.LocalPlayer.SetHeading((_lastRotInMap + 180) % 360);
        }

        private void RefreshInfoKillAfterTime()
        {
            if (_info == null)
                _info = new DxText(string.Format(Settings.Language.OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME, _outsideCounter.ToString()), 0.5f, 0.1f, 1f, Color.White,
                    alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Top);
            else
                _info.Text = string.Format(Settings.Language.OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME, _outsideCounter.ToString());
        }

        private void RefreshInfoTeleportAfterTime()
        {
            if (_info == null)
                _info = new DxText(string.Format(Settings.Language.OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME, _outsideCounter.ToString()), 0.5f, 0.1f, 1f, Color.White,
                    alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Top);
            else
                _info.Text = string.Format(Settings.Language.OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME, _outsideCounter.ToString());
        }

        private bool IsWithin() => IsWithin(Player.LocalPlayer.Position);

        private bool IsWithin(Vector3 point)
        {
            if (point.X < _minX || point.Y < _minY || point.X > _maxX || point.Y > _maxY)
                return false;

            bool inside = false;
            for (int i = 0, j = _edges.Count - 1; i < _edges.Count; j = i++)
            {
                Position4DDto iPoint = _edges[i];
                Position4DDto jPoint = _edges[j];
                bool intersect = ((iPoint.Y > point.Y) != (jPoint.Y > point.Y))
                        && (point.X < (jPoint.X - iPoint.X) * (point.Y - iPoint.Y) / (jPoint.Y - iPoint.Y) + iPoint.X);
                if (intersect)
                    inside = !inside;
            }
            return inside;
        }

        private void DrawGpsRoutes()
        {
            if (_createdGpsRoutes)
                return;
            if (_edges.Count == 0)
                return;

            /*// START_GPS_CUSTOM_ROUTE
            Invoker.Invoke(0xDB34E8D56FC13B08, 6, false, true);

            foreach (var edge in _edges)
            {
                // ADD_POINT_TO_GPS_CUSTOM_ROUTE
                Invoker.Invoke(0x311438A071DD9B1A, edge.X, edge.Y, edge.Z);
            }

            // SET_GPS_CUSTOM_ROUTE_RENDER
            RAGE.Game.Invoker.Invoke(0x900086F371220B6F, true, 16, 16);*/

            _createdGpsRoutes = true;
        }

        private void ClearGpsRoutes()
        {
            if (!_createdGpsRoutes)
                return;

            // CLEAR_GPS_CUSTOM_ROUTE
            //RAGE.Game.Invoker.Invoke(0xE6DE0561D9232A64);

            _createdGpsRoutes = false;
        }

        private void Draw()
        {
            float totalMaxTop = -1;
            for (int i = 0; i < _edges.Count; ++i)
            {
                var edgeStart = _edges[i];
                var edgeTarget = i == _edges.Count - 1 ? _edges[0] : _edges[i+1];
                float edgeStartZ = 0;
                float edgeTargetZ = 0;
                Misc.GetGroundZFor3dCoord(edgeStart.X, edgeStart.Y, Player.LocalPlayer.Position.Z, ref edgeStartZ, false);
                Misc.GetGroundZFor3dCoord(edgeTarget.X, edgeTarget.Y, Player.LocalPlayer.Position.Z, ref edgeTargetZ, false);

                //var textureRes = Graphics.GetTextureResolution("commonmenu", "gradient_bgd");
                //Graphics.Draw  .DrawSprite("commonmenu", "gradient_bgd", )

                Color color = Settings.MapBorderColor;
                float maxTop = Math.Max(edgeStartZ + 50, edgeTargetZ + 50);
                totalMaxTop = Math.Max(totalMaxTop, maxTop);
                if (_edgesMaxTop != -1)
                    maxTop = _edgesMaxTop;
                Graphics.DrawPoly(edgeTarget.X, edgeTarget.Y, maxTop, edgeTarget.X, edgeTarget.Y, edgeTargetZ, edgeStart.X, edgeStart.Y, edgeStartZ, color.R, color.G, color.B, color.A);
                Graphics.DrawPoly(edgeStart.X, edgeStart.Y, edgeStartZ, edgeStart.X, edgeStart.Y, maxTop, edgeTarget.X, edgeTarget.Y, maxTop, color.R, color.G, color.B, color.A);

                Graphics.DrawPoly(edgeStart.X, edgeStart.Y, maxTop, edgeStart.X, edgeStart.Y, edgeStartZ, edgeTarget.X, edgeTarget.Y, edgeTargetZ, color.R, color.G, color.B, color.A);
                Graphics.DrawPoly(edgeTarget.X, edgeTarget.Y, edgeTargetZ, edgeTarget.X, edgeTarget.Y, maxTop, edgeStart.X, edgeStart.Y, maxTop, color.R, color.G, color.B, color.A);


                /*Graphics.DrawLine(edgeStart.X, edgeStart.Y, edgeStartZ - 0.5f, edgeTarget.X, edgeTarget.Y, edgeTargetZ - 0.5f, 150, 0, 0, 255);
                Graphics.DrawLine(edgeStart.X, edgeStart.Y, edgeStartZ + 0.5f, edgeTarget.X, edgeTarget.Y, edgeTargetZ + 0.5f, 150, 0, 0, 255);
                Graphics.DrawLine(edgeStart.X, edgeStart.Y, edgeStartZ + 1.5f, edgeTarget.X, edgeTarget.Y, edgeTargetZ + 1.5f, 150, 0, 0, 255);
                Graphics.DrawLine(edgeStart.X, edgeStart.Y, edgeStartZ + 2.5f, edgeTarget.X, edgeTarget.Y, edgeTargetZ + 2.5f, 150, 0, 0, 255);*/
            }

            _edgesMaxTop = totalMaxTop;
        }
    }
}