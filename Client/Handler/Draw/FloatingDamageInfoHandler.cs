using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Entities.Draw;
using TDS_Client.Handler.Events;

namespace TDS_Client.Handler.Draw
{
    public class FloatingDamageInfoHandler : ServiceBase
    {
        #region Private Fields

        private readonly DxHandler _dxHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly TimerHandler _timerHandler;
        private List<FloatingDamageInfo> _damageInfos = new List<FloatingDamageInfo>();

        #endregion Private Fields

        #region Public Constructors

        public FloatingDamageInfoHandler(IModAPI modAPI, LoggingHandler loggingHandler, TimerHandler timerHandler, SettingsHandler settingsHandler,
            EventsHandler eventsHandler, DxHandler dxHandler)
            : base(modAPI, loggingHandler)
        {
            _timerHandler = timerHandler;
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;

            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(UpdateAllPositions, () => _damageInfos.Count > 0));

            eventsHandler.InFightStatusChanged += EventsHandler_InFightStatusChanged;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Add(IPlayer target, float damage)
        {
            var info = new FloatingDamageInfo(target, damage, _timerHandler.ElapsedMs, ModAPI, _settingsHandler, _dxHandler, _timerHandler);
            _damageInfos.Add(info);
        }

        public void Clear()
        {
            foreach (var info in _damageInfos)
            {
                info.Remove();
            }
            _damageInfos.Clear();
        }

        #endregion Public Methods

        #region Private Methods

        private void EventsHandler_InFightStatusChanged(bool inFight)
        {
            if (inFight)
                UpdateAllPositions(0);
            else
                Clear();
        }

        private void UpdateAllPositions(int currentMs)
        {
            _damageInfos.RemoveAll(x => x.RemoveAtHandler);
            foreach (var damageInfo in _damageInfos)
            {
                damageInfo.UpdatePosition(currentMs);
            }
        }

        #endregion Private Methods
    }
}
