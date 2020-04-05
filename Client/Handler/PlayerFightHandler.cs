using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Handler.Events;

namespace TDS_Client.Handler
{
    public class PlayerFightHandler
    {
        public bool InFight
        {
            get => _inFight;
            set
            {
                if (_inFight == value)
                    return;
                _inFight = value;
                _eventsHandler.OnInFightStatusChanged(value);

                if (value)
                {
                    MapLimitManager.Start();
                    if (!_inFight)
                    {
                        FightInfo.Reset();
                        FloatingDamageInfo.UpdateAllPositions();
                        FiringMode.Start();
                        Browser.Angular.Main.ToggleRoundStats(true);
                    }
                }
                else
                {
                    MapLimitManager.Stop();
                    if (_inFight)
                    {
                        FloatingDamageInfo.RemoveAll();
                        FiringMode.Stop();
                        Browser.Angular.Main.ToggleRoundStats(false);
                    }
                }
                _inFight = value;
            }
        }

        private bool _inFight;

        private readonly EventsHandler _eventsHandler;

        public PlayerFightHandler(EventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;
        }
    }
}
