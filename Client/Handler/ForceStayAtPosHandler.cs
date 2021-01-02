using RAGE;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Handler.Draw.Dx;
using TDS.Client.Handler.Entities;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Default;

namespace TDS.Client.Handler
{
    public class ForceStayAtPosHandler : ServiceBase
    {
        private readonly DxHandler _dxHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        private readonly SettingsHandler _settingsHandler;
        private readonly TimerHandler _timerHandler;
        private MapLimit _mapLimit;

        public ForceStayAtPosHandler(LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender, SettingsHandler settingsHandler, DxHandler dxHandler, TimerHandler timerHandler)
            : base(loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;

            RAGE.Events.Add(ToClientEvent.SetForceStayAtPosition, OnSetForceStayAtPositionMethod);
            RAGE.Events.Add(ToClientEvent.RemoveForceStayAtPosition, OnRemoveForceStayAtPositionMethod);
        }

        public void Start(Position3D pos, float radius, MapLimitType type, int allowedTimeOut = 0)
        {
            _mapLimit?.Stop();

            var edges = new List<Vector3>
            {
                new Vector3 { X = pos.X - radius, Y = pos.Y, Z = pos.Z },  // left
                new Vector3 { X = pos.X - radius/2, Y = pos.Y - radius/2, Z = pos.Z },  // left top
                new Vector3 { X = pos.X, Y = pos.Y - radius, Z = pos.Z },  // top
                new Vector3 { X = pos.X + radius/2, Y = pos.Y - radius/2, Z = pos.Z },  // top right
                new Vector3 { X = pos.X + radius, Y = pos.Y, Z = pos.Z },  // right
                new Vector3 { X = pos.X + radius/2, Y = pos.Y + radius/2, Z = pos.Z },  // right bottom
                new Vector3 { X = pos.X, Y = pos.Y + radius, Z = pos.Z },  // bottom
                new Vector3 { X = pos.X - radius/2, Y = pos.Y + radius/2, Z = pos.Z },  // bottom left
            };
            _mapLimit = new MapLimit(edges, type, allowedTimeOut, Color.FromArgb(30, 255, 255, 255), _remoteEventsSender, _settingsHandler, _dxHandler, _timerHandler);
            _mapLimit.Start();
        }

        public void Stop()
        {
            _mapLimit?.Stop();
            _mapLimit = null;
        }

        private void OnRemoveForceStayAtPositionMethod(object[] args)
        {
            Stop();
        }

        private void OnSetForceStayAtPositionMethod(object[] args)
        {
            var pos = Serializer.FromServer<Position3D>(Convert.ToString(args[0]));
            var radius = Convert.ToSingle(args[1]);
            var type = args.Length >= 3 ? (MapLimitType)Convert.ToInt32(args[2]) : MapLimitType.Block;
            var allowedTimeOut = args.Length >= 4 ? Convert.ToInt32(args[3]) : 0;

            Start(pos, radius, type, allowedTimeOut);
        }
    }
}