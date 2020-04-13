using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class ForceStayAtPosHandler
    {
        private MapLimit _mapLimit;

        private readonly IModAPI _modAPI;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;
        private readonly DxHandler _dxHandler;
        private readonly TimerHandler _timerHandler;
        private readonly Serializer _serializer;

        public ForceStayAtPosHandler(IModAPI modAPI, RemoteEventsSender remoteEventsSender, SettingsHandler settingsHandler, DxHandler dxHandler, TimerHandler timerHandler,
            Serializer serializer)
        {
            _modAPI = modAPI;
            _remoteEventsSender = remoteEventsSender;
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;
            _serializer = serializer;

            modAPI.Event.Add(ToClientEvent.SetForceStayAtPosition, OnSetForceStayAtPositionMethod);
            modAPI.Event.Add(ToClientEvent.RemoveForceStayAtPosition, OnRemoveForceStayAtPositionMethod);
        }

        public void Start(Position3D pos, float radius, MapLimitType type, int allowedTimeOut = 0)
        {
            _mapLimit?.Stop();

            var edges = new List<Position3D>
            {
                new Position3D { X = pos.X - radius, Y = pos.Y, Z = pos.Z },  // left 
                new Position3D { X = pos.X - radius/2, Y = pos.Y - radius/2, Z = pos.Z },  // left top
                new Position3D { X = pos.X, Y = pos.Y - radius, Z = pos.Z },  // top 
                new Position3D { X = pos.X + radius/2, Y = pos.Y - radius/2, Z = pos.Z },  // top right 
                new Position3D { X = pos.X + radius, Y = pos.Y, Z = pos.Z },  // right 
                new Position3D { X = pos.X + radius/2, Y = pos.Y + radius/2, Z = pos.Z },  // right bottom 
                new Position3D { X = pos.X, Y = pos.Y + radius, Z = pos.Z },  // bottom 
                new Position3D { X = pos.X - radius/2, Y = pos.Y + radius/2, Z = pos.Z },  // bottom left  
            };
            _mapLimit = new MapLimit(edges, type, allowedTimeOut, Color.FromArgb(30, 255, 255, 255), _modAPI, _remoteEventsSender, _settingsHandler, _dxHandler, _timerHandler);
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
            var pos = _serializer.FromServer<Position3D>(Convert.ToString(args[0]));
            var radius = Convert.ToSingle(args[1]);
            var type = args.Length >= 3 ? (MapLimitType)Convert.ToInt32(args[2]) : MapLimitType.Block;
            var allowedTimeOut = args.Length >= 4 ? Convert.ToInt32(args[3]) : 0;

            Start(pos, radius, type, allowedTimeOut);
        }
    }
}
