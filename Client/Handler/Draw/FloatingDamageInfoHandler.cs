using System;
using System.Collections.Generic;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Handler.Draw.Dx;
using TDS.Client.Handler.Entities.Draw;
using TDS.Client.Handler.Events;
using static RAGE.Events;

namespace TDS.Client.Handler.Draw
{
    public class FloatingDamageInfoHandler : ServiceBase
    {
        private readonly DxHandler _dxHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly TimerHandler _timerHandler;

        private readonly List<FloatingDamageInfo> _damageInfos = new List<FloatingDamageInfo>();

        public FloatingDamageInfoHandler(LoggingHandler loggingHandler, TimerHandler timerHandler, SettingsHandler settingsHandler,
            EventsHandler eventsHandler, DxHandler dxHandler)
            : base(loggingHandler)
        {
            _timerHandler = timerHandler;
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;

            eventsHandler.InFightStatusChanged += EventsHandler_InFightStatusChanged;
        }

        public void Add(ITDSPlayer target, float damage)
        {
            var info = new FloatingDamageInfo(target, damage, _timerHandler.ElapsedMs, _settingsHandler, _dxHandler, _timerHandler);
            _damageInfos.Add(info);
            CheckAddEvent(true);
        }

        public void Clear()
        {
            if (_damageInfos.Count == 0)
                return;

            foreach (var info in _damageInfos)
            {
                info.Remove();
            }
            _damageInfos.Clear();
            CheckAddEvent(false);
        }

        private void EventsHandler_InFightStatusChanged(bool inFight)
        {
            if (inFight)
                UpdateAllPositions(null);
            else
                Clear();
        }

        private void UpdateAllPositions(List<TickNametagData> _)
        {
            if (_damageInfos.Count == 0)
                return;

            var currentMs = _timerHandler.ElapsedMs;
            _damageInfos.RemoveAll(x => x.RemoveAtHandler);
            foreach (var damageInfo in _damageInfos)
            {
                damageInfo.UpdatePosition(currentMs);
            }
            CheckAddEvent(false);
        }

        private void CheckAddEvent(bool added)
        {
            if (added && _damageInfos.Count == 1)
            {
                Tick += UpdateAllPositions;
            }
            else if (!added && _damageInfos.Count == 0)
            {
                Tick -= UpdateAllPositions;
            }
        }
    }
}
