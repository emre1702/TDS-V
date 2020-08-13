using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Entities
{
    public class MapLimit
    {
        #region Public Fields

        public Color MapBorderColor;

        #endregion Public Fields

        #region Private Fields

        private readonly DxHandler _dxHandler;
        private readonly Dictionary<MapLimitType, Action> _mapLimitTypeMethod = new Dictionary<MapLimitType, Action> { };
        private readonly int _maxOutsideCounter;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;
        private readonly EventMethodData<TickDelegate> _tickEventMethod;
        private readonly TimerHandler _timerHandler;
        private readonly HashSet<MapLimitType> _typeToCheckFaster = new HashSet<MapLimitType> { MapLimitType.Block };
        private readonly IModAPI ModAPI;
        private TDSTimer _checkTimer;
        private TDSTimer _checkTimerFaster;
        private bool _createdGpsRoutes;
        private List<Position> _edges;
        private float _edgesMaxTop = -1;
        private DxText _info;
        private Position _lastPosInMap;
        private float _lastRotInMap;
        private float _minX, _minY, _maxX, _maxY;
        private int _outsideCounter;
        private bool _started;
        private MapLimitType _type;

        #endregion Private Fields

        #region Public Constructors

        public MapLimit(List<Position> edges, MapLimitType type, int maxOutsideCounter, Color mapBorderColor, IModAPI modAPI, RemoteEventsSender remoteEventsSender,
            SettingsHandler settingsHandler, DxHandler dxHandler, TimerHandler timerHandler)
        {
            ModAPI = modAPI;
            _remoteEventsSender = remoteEventsSender;
            _tickEventMethod = new EventMethodData<TickDelegate>(Draw);
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

        #endregion Public Constructors

        #region Private Properties

        private bool SavePosition => _edges != null && (_type == MapLimitType.Block || _type == MapLimitType.TeleportBackAfterTime);

        #endregion Private Properties

        #region Public Methods

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

        public void SetEdges(List<Position> edges)
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
                if (ModAPI.Misc.GetGroundZFor3dCoord(edge.X, edge.Y, edge.Z + 1, ref edgeZ))
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
            ModAPI.Event.Tick.Add(_tickEventMethod);
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
            ModAPI.Event.Tick.Remove(_tickEventMethod);
            ClearGpsRoutes();
            _started = false;
        }

        #endregion Public Methods

        #region Private Methods

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

            ModAPI.Native.Invoke(NativeHash.CLEAR_GPS_CUSTOM_ROUTE);

            _createdGpsRoutes = false;
        }

        private void Draw(int _)
        {
            float totalMaxTop = -1;
            for (int i = 0; i < _edges.Count; ++i)
            {
                var edgeStart = _edges[i];
                var edgeTarget = i == _edges.Count - 1 ? _edges[0] : _edges[i + 1];
                float edgeStartZ = 0;
                float edgeTargetZ = 0;
                ModAPI.Misc.GetGroundZFor3dCoord(edgeStart.X, edgeStart.Y, ModAPI.LocalPlayer.Position.Z, ref edgeStartZ);
                ModAPI.Misc.GetGroundZFor3dCoord(edgeTarget.X, edgeTarget.Y, ModAPI.LocalPlayer.Position.Z, ref edgeTargetZ);

                //var textureRes = Graphics.GetTextureResolution("commonmenu", "gradient_bgd");
                //Graphics.Draw  .DrawSprite("commonmenu", "gradient_bgd", )

                Color color = MapBorderColor;
                float maxTop = Math.Max(edgeStartZ + 50, edgeTargetZ + 50);
                totalMaxTop = Math.Max(totalMaxTop, maxTop);
                if (_edgesMaxTop != -1)
                    maxTop = _edgesMaxTop;
                ModAPI.Graphics.DrawPoly(edgeTarget.X, edgeTarget.Y, maxTop, edgeTarget.X, edgeTarget.Y, edgeTargetZ, edgeStart.X, edgeStart.Y, edgeStartZ, color.R, color.G, color.B, color.A);
                ModAPI.Graphics.DrawPoly(edgeStart.X, edgeStart.Y, edgeStartZ, edgeStart.X, edgeStart.Y, maxTop, edgeTarget.X, edgeTarget.Y, maxTop, color.R, color.G, color.B, color.A);

                ModAPI.Graphics.DrawPoly(edgeStart.X, edgeStart.Y, maxTop, edgeStart.X, edgeStart.Y, edgeStartZ, edgeTarget.X, edgeTarget.Y, edgeTargetZ, color.R, color.G, color.B, color.A);
                ModAPI.Graphics.DrawPoly(edgeTarget.X, edgeTarget.Y, edgeTargetZ, edgeTarget.X, edgeTarget.Y, maxTop, edgeStart.X, edgeStart.Y, maxTop, color.R, color.G, color.B, color.A);

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
            ModAPI.LocalPlayer.Position = _lastPosInMap;
            ModAPI.LocalPlayer.Heading = (_lastRotInMap + 180) % 360;
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
                ModAPI.LocalPlayer.Position = _lastPosInMap;
                Reset();
            }
        }

        private bool IsWithin() => IsWithin(ModAPI.LocalPlayer.Position);

        private bool IsWithin(Position point)
        {
            if (point.X < _minX || point.Y < _minY || point.X > _maxX || point.Y > _maxY)
                return false;

            bool inside = false;
            for (int i = 0, j = _edges.Count - 1; i < _edges.Count; j = i++)
            {
                Position iPoint = _edges[i];
                Position jPoint = _edges[j];
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
                _info = new DxText(_dxHandler, ModAPI, _timerHandler, string.Format(_settingsHandler.Language.OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME, _outsideCounter.ToString()), 0.5f, 0.1f, 1f,
                    Color.White, alignmentX: AlignmentX.Center, alignmentY: AlignmentY.Top);
            else
                _info.Text = string.Format(_settingsHandler.Language.OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME, _outsideCounter.ToString());
        }

        private void RefreshInfoTeleportAfterTime()
        {
            if (_info == null)
                _info = new DxText(_dxHandler, ModAPI, _timerHandler, string.Format(_settingsHandler.Language.OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME, _outsideCounter.ToString()), 0.5f, 0.1f, 1f,
                    Color.White, alignmentX: AlignmentX.Center, alignmentY: AlignmentY.Top);
            else
                _info.Text = string.Format(_settingsHandler.Language.OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME, _outsideCounter.ToString());
        }

        private void Reset()
        {
            if (SavePosition)
            {
                _lastPosInMap = ModAPI.LocalPlayer.Position;
                _lastRotInMap = ModAPI.LocalPlayer.Heading;
            }
            if (_outsideCounter == _maxOutsideCounter)
                return;
            _info?.Remove();
            _info = null;
            _outsideCounter = _maxOutsideCounter;
        }

        #endregion Private Methods
    }
}
