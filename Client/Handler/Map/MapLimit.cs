using RAGE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Draw.Dx;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Default;
using static RAGE.Events;
using static RAGE.NUI.UIResText;

namespace TDS.Client.Handler.Entities
{
    public class MapLimit
    {
        public Color MapBorderColor;

        private readonly DxHandler _dxHandler;
        private readonly Dictionary<MapLimitType, Action> _mapLimitTypeMethod = new Dictionary<MapLimitType, Action> { };
        private readonly int _maxOutsideCounter;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;
        private readonly TimerHandler _timerHandler;
        private readonly HashSet<MapLimitType> _typeToCheckFaster = new HashSet<MapLimitType> { MapLimitType.Block };

        private TDSTimer _checkTimer;
        private TDSTimer _checkTimerFaster;
        private bool _createdGpsRoutes;
        private List<Vector3> _edges;
        private float _edgesMaxTop = -1;
        private DxText _info;
        private Vector3 _lastPosInMap;
        private float _lastRotInMap;
        private float _minX, _minY, _maxX, _maxY;
        private int _outsideCounter;
        private bool _started;
        private MapLimitType _type;

        public MapLimit(List<Vector3> edges, MapLimitType type, int maxOutsideCounter, Color mapBorderColor, RemoteEventsSender remoteEventsSender,
            SettingsHandler settingsHandler, DxHandler dxHandler, TimerHandler timerHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;

            _type = type;
            _maxOutsideCounter = maxOutsideCounter;
            MapBorderColor = mapBorderColor;

            SetEdges(edges);

            _mapLimitTypeMethod[MapLimitType.KillAfterTime] = IsOutsideKillAfterTime;
            _mapLimitTypeMethod[MapLimitType.TeleportBackAfterTime] = IsOutsideTeleportBackAfterTime;
            _mapLimitTypeMethod[MapLimitType.Block] = IsOutsideBlock;
        }

        private bool SavePosition => _edges != null && (_type == MapLimitType.Block || _type == MapLimitType.TeleportBackAfterTime);

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

        public void SetEdges(List<Vector3> edges)
        {
            if (_type != MapLimitType.Display)
            {
                _minX = edges.Count > 0 ? edges.Min(v => v.X) : 0;
                _minY = edges.Count > 0 ? edges.Min(v => v.Y) : 0;
                _maxX = edges.Count > 0 ? edges.Max(v => v.X) : 0;
                _maxY = edges.Count > 0 ? edges.Max(v => v.Y) : 0;
            }

            foreach (var edge in edges)
            {
                float edgeZ = 0;
                if (RAGE.Game.Misc.GetGroundZFor3dCoord(edge.X, edge.Y, edge.Z + 1, ref edgeZ, false))
                    edge.Z = edgeZ;
            }

            _edges = edges;

            if (_started)
            {
                ClearGpsRoutes();
                DrawGpsRoutes();
            }
        }

        public void SetType(MapLimitType type, bool ignoreEqual = false)
        {
            if (_type == type && !ignoreEqual)
                return;
            _type = type;
            _checkTimerFaster?.Kill();
            _checkTimerFaster = null;
            _checkTimer?.Kill();
            _checkTimer = null;

            if (_type != MapLimitType.Display)
            {
                _minX = _edges.Count > 0 ? _edges.Min(v => v.X) : 0;
                _minY = _edges.Count > 0 ? _edges.Min(v => v.Y) : 0;
                _maxX = _edges.Count > 0 ? _edges.Max(v => v.X) : 0;
                _maxY = _edges.Count > 0 ? _edges.Max(v => v.Y) : 0;

                if (_typeToCheckFaster.Contains(_type))
                    _checkTimerFaster = new TDSTimer(CheckFaster, Constants.MapLimitFasterCheckTimeMs, 0);
                else
                    _checkTimer = new TDSTimer(Check, 1000, 0);
            }
        }

        public void Start()
        {
            if (_started)
                Stop();
            Reset();
            SetType(_type, true);
            Tick += Draw;
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
            _outsideCounter = _maxOutsideCounter;
            Tick -= Draw;
            ClearGpsRoutes();
            _started = false;
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

        private void ClearGpsRoutes()
        {
            if (!_createdGpsRoutes)
                return;

            RAGE.Game.Invoker.Invoke((ulong)NativeHash.CLEAR_GPS_CUSTOM_ROUTE);

            _createdGpsRoutes = false;
        }

        private void Draw(List<TickNametagData> _)
        {
            float totalMaxTop = -1;
            for (int i = 0; i < _edges.Count; ++i)
            {
                var edgeStart = _edges[i];
                var edgeTarget = i == _edges.Count - 1 ? _edges[0] : _edges[i + 1];
                float edgeStartZ = 0;
                float edgeTargetZ = 0;
                RAGE.Game.Misc.GetGroundZFor3dCoord(edgeStart.X, edgeStart.Y, RAGE.Elements.Player.LocalPlayer.Position.Z, ref edgeStartZ, false);
                RAGE.Game.Misc.GetGroundZFor3dCoord(edgeTarget.X, edgeTarget.Y, RAGE.Elements.Player.LocalPlayer.Position.Z, ref edgeTargetZ, false);

                //var textureRes = Graphics.GetTextureResolution("commonmenu", "gradient_bgd");
                //Graphics.Draw  .DrawSprite("commonmenu", "gradient_bgd", )

                Color color = MapBorderColor;
                float maxTop = Math.Max(edgeStartZ + 50, edgeTargetZ + 50);
                totalMaxTop = Math.Max(totalMaxTop, maxTop);
                if (_edgesMaxTop != -1)
                    maxTop = _edgesMaxTop;
                RAGE.Game.Graphics.DrawPoly(edgeTarget.X, edgeTarget.Y, maxTop, edgeTarget.X, edgeTarget.Y, edgeTargetZ, edgeStart.X, edgeStart.Y, edgeStartZ, color.R, color.G, color.B, color.A);
                RAGE.Game.Graphics.DrawPoly(edgeStart.X, edgeStart.Y, edgeStartZ, edgeStart.X, edgeStart.Y, maxTop, edgeTarget.X, edgeTarget.Y, maxTop, color.R, color.G, color.B, color.A);

                RAGE.Game.Graphics.DrawPoly(edgeStart.X, edgeStart.Y, maxTop, edgeStart.X, edgeStart.Y, edgeStartZ, edgeTarget.X, edgeTarget.Y, edgeTargetZ, color.R, color.G, color.B, color.A);
                RAGE.Game.Graphics.DrawPoly(edgeTarget.X, edgeTarget.Y, edgeTargetZ, edgeTarget.X, edgeTarget.Y, maxTop, edgeStart.X, edgeStart.Y, maxTop, color.R, color.G, color.B, color.A);

                /*Graphics.DrawLine(edgeStart.X, edgeStart.Y, edgeStartZ - 0.5f, edgeTarget.X, edgeTarget.Y, edgeTargetZ - 0.5f, 150, 0, 0, 255);
                Graphics.DrawLine(edgeStart.X, edgeStart.Y, edgeStartZ + 0.5f, edgeTarget.X, edgeTarget.Y, edgeTargetZ + 0.5f, 150, 0, 0, 255);
                Graphics.DrawLine(edgeStart.X, edgeStart.Y, edgeStartZ + 1.5f, edgeTarget.X, edgeTarget.Y, edgeTargetZ + 1.5f, 150, 0, 0, 255);
                Graphics.DrawLine(edgeStart.X, edgeStart.Y, edgeStartZ + 2.5f, edgeTarget.X, edgeTarget.Y, edgeTargetZ + 2.5f, 150, 0, 0, 255);*/
            }

            _edgesMaxTop = totalMaxTop;
        }

        private void DrawGpsRoutes()
        {
            if (_createdGpsRoutes)
                return;
            if (_edges.Count == 0)
                return;

            // Doesn't work
            /*// START_GPS_CUSTOM_ROUTE
            Invoker.Invoke(Natives._0xDB34E8D56FC13B08, 6, false, true);

            foreach (var edge in _edges)
            {
                // ADD_POINT_TO_GPS_CUSTOM_ROUTE
                Invoker.Invoke(Natives._0x311438A071DD9B1A, edge.X, edge.Y, edge.Z);
            }

            // SET_GPS_CUSTOM_ROUTE_RENDER
            Invoker.Invoke(Natives._0x900086F371220B6F, true, 16, 16); */

            _createdGpsRoutes = true;
        }

        private void IsOutsideBlock()
        {
            RAGE.Elements.Player.LocalPlayer.Position = _lastPosInMap;
            RAGE.Elements.Player.LocalPlayer.SetRotation(0, 0, (_lastRotInMap + 180) % 360, 2, true);
        }

        private void IsOutsideKillAfterTime()
        {
            if (_outsideCounter > 0)
                RefreshInfoKillAfterTime();
            else
            {
                _remoteEventsSender.Send(ToServerEvent.OutsideMapLimit);
                Stop();
            }
        }

        private void IsOutsideTeleportBackAfterTime()
        {
            if (_outsideCounter > 0)
                RefreshInfoTeleportAfterTime();
            else
            {
                RAGE.Elements.Player.LocalPlayer.Position = _lastPosInMap;
                Reset();
            }
        }

        private bool IsWithin() => IsWithin(RAGE.Elements.Player.LocalPlayer.Position);

        private bool IsWithin(Vector3 point)
        {
            if (point.X < _minX || point.Y < _minY || point.X > _maxX || point.Y > _maxY)
                return false;

            bool inside = false;
            for (int i = 0, j = _edges.Count - 1; i < _edges.Count; j = i++)
            {
                var iPoint = _edges[i];
                var jPoint = _edges[j];
                bool intersect = ((iPoint.Y > point.Y) != (jPoint.Y > point.Y))
                        && (point.X < (jPoint.X - iPoint.X) * (point.Y - iPoint.Y) / (jPoint.Y - iPoint.Y) + iPoint.X);
                if (intersect)
                    inside = !inside;
            }
            return inside;
        }

        private void RefreshInfoKillAfterTime()
        {
            if (_info == null)
                _info = new DxText(_dxHandler, _timerHandler, string.Format(_settingsHandler.Language.OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME, _outsideCounter.ToString()), 0.5f, 0.1f, 1f,
                    Color.White, Alignment: Alignment.Centered, alignmentY: AlignmentY.Top);
            else
                _info.Text = string.Format(_settingsHandler.Language.OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME, _outsideCounter.ToString());
        }

        private void RefreshInfoTeleportAfterTime()
        {
            if (_info == null)
                _info = new DxText(_dxHandler, _timerHandler, string.Format(_settingsHandler.Language.OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME, _outsideCounter.ToString()), 0.5f, 0.1f, 1f,
                    Color.White, Alignment: Alignment.Centered, alignmentY: AlignmentY.Top);
            else
                _info.Text = string.Format(_settingsHandler.Language.OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME, _outsideCounter.ToString());
        }

        private void Reset()
        {
            if (SavePosition)
            {
                _lastPosInMap = RAGE.Elements.Player.LocalPlayer.Position;
                _lastRotInMap = RAGE.Elements.Player.LocalPlayer.GetHeading();
            }
            if (_outsideCounter == _maxOutsideCounter)
                return;
            _info?.Remove();
            _info = null;
            _outsideCounter = _maxOutsideCounter;
        }
    }
}
