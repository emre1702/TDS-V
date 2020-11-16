using RAGE;
using System.Drawing;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Draw.Dx;
using static RAGE.NUI.UIResText;

namespace TDS.Client.Handler.Entities.Draw
{
    public class FloatingDamageInfo
    {
        public bool RemoveAtHandler;

        private readonly float _damage;
        private readonly DxHandler _dxHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly int _startTicks;
        private readonly Vector3 _targetPosition;
        private readonly TimerHandler _timerHandler;

        private DxText _text;

        internal FloatingDamageInfo(ITDSPlayer target, float damage, int currentMs, SettingsHandler settingsHandler, DxHandler dxHandler, TimerHandler timerHandler)
        {
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;

            _damage = damage;
            _startTicks = currentMs;
            _targetPosition = target.Position;
        }

        public void Remove()
        {
            _text?.Remove();
        }

        public void UpdatePosition(long currentMs)
        {
            int elapsedTicks = (int)(currentMs - _startTicks);
            if (elapsedTicks > _settingsHandler.PlayerSettings.ShowFloatingDamageInfoDurationMs)
            {
                RemoveAtHandler = true;
                Remove();
                return;
            }
            float screenX = 0;
            float screenY = 0;
            RAGE.Game.Graphics.GetScreenCoordFromWorldCoord(_targetPosition.X, _targetPosition.Y, _targetPosition.Z, ref screenX, ref screenY);

            float percentage = (float)elapsedTicks / _settingsHandler.PlayerSettings.ShowFloatingDamageInfoDurationMs;
            screenY -= 0.2f * percentage;

            float scale = 0.4f - (0.3f * percentage);
            var color = Color.FromArgb(255 - (int)(255 * percentage), 220, 220, 220);

            if (_text == null)
                _text = new DxText(_dxHandler, _timerHandler, _damage.ToString(), screenX, screenY, scale, color, Alignment: Alignment.Centered, alignmentY: AlignmentY.Bottom, dropShadow: false, outline: true);
            else
                _text.SetRelativeY(screenY);
        }
    }
}
