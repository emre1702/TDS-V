using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Event
{
    public class TickNametagEventHandler : BaseEventHandler<TickNametagDelegate>
    {
        private readonly PlayerConvertingHandler _playerConvertingHandler;

        public TickNametagEventHandler(PlayerConvertingHandler playerConvertingHandler)
        {
            _playerConvertingHandler = playerConvertingHandler;

            RAGE.Events.Tick += OnTick;
        }

        private void OnTick(List<RAGE.Events.TickNametagData> nametags)
        {
            var newNametags = new List<TickNametagData>();
            foreach (var nametag in nametags)
                newNametags.Add(new TickNametagData 
                { 
                    Player = _playerConvertingHandler.GetPlayer(nametag.Player),
                    ScreenX = nametag.ScreenX,
                    ScreenY = nametag.ScreenY,
                    Distance = nametag.Distance
                });

            foreach (var a in Actions)
                if (a.Requirement is null || a.Requirement())
                    a.Method(newNametags);
        }
    }
}
