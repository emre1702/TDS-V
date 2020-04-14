using System;
using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Event
{
    public class TickNametagEventHandler : BaseEventHandler<TickNametagDelegate>
    {
        private readonly LoggingHandler _loggingHandler;
        private readonly PlayerConvertingHandler _playerConvertingHandler;

        public TickNametagEventHandler(LoggingHandler loggingHandler, PlayerConvertingHandler playerConvertingHandler)
        {
            _loggingHandler = loggingHandler;
            _playerConvertingHandler = playerConvertingHandler;

            RAGE.Events.Tick += OnTick;
        }

        private void OnTick(List<RAGE.Events.TickNametagData> nametags)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                var newNametags = new List<TickNametagData>();
                if (nametags != null)
                {
                    foreach (var nametag in nametags)
                    {
                        if (nametag.Player is null)
                            continue;
                        newNametags.Add(new TickNametagData
                        {
                            Player = _playerConvertingHandler.GetPlayer(nametag.Player),
                            ScreenX = nametag.ScreenX,
                            ScreenY = nametag.ScreenY,
                            Distance = nametag.Distance
                        });
                    }
                    
                }

                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(newNametags);
                }
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }
    }
}
