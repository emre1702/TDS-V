using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Draw.Dx;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.Entities.Draw
{
    public class FloatingDamageInfo
    {
        public bool RemoveAtHandler;

        private readonly float _damage;
        private readonly int _startTicks;
        private readonly Position3D _targetPosition;
        private DxText _text;

        private readonly IModAPI _modAPI;
        private readonly SettingsHandler _settingsHandler;
        private readonly DxHandler _dxHandler;
        private readonly TimerHandler _timerHandler;

        internal FloatingDamageInfo(IPlayer target, float damage, int currentMs, IModAPI modAPI, SettingsHandler settingsHandler, DxHandler dxHandler, TimerHandler timerHandler)
        {
            _modAPI = modAPI;
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;

            _damage = damage;
            _startTicks = currentMs;
            _targetPosition = target.Position;
        }

        public void UpdatePosition(int currentMs)
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
            _modAPI.Graphics.GetScreenCoordFromWorldCoord(_targetPosition.X, _targetPosition.Y, _targetPosition.Z + 1, ref screenX, ref screenY);

            float percentage = elapsedTicks / _settingsHandler.PlayerSettings.ShowFloatingDamageInfoDurationMs;
            screenY -= percentage * 0.05f;

            if (_text == null)
                _text = new DxText(_dxHandler, _modAPI, _timerHandler, _damage.ToString(), screenX, screenY, 0.4f, Color.White, alignmentX: AlignmentX.Center, alignmentY: AlignmentY.Bottom, dropShadow: true, outline: true);
            else
                _text.SetRelativeY(screenY);
        }

        public void Remove()
        {
            _text?.Remove();
        }
    }
}
