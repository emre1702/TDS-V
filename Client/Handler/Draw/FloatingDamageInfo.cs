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
        #region Public Fields

        public bool RemoveAtHandler;

        #endregion Public Fields

        #region Private Fields

        private readonly float _damage;
        private readonly DxHandler _dxHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly int _startTicks;
        private readonly Position3D _targetPosition;
        private readonly TimerHandler _timerHandler;
        private readonly IModAPI ModAPI;
        private DxText _text;

        #endregion Private Fields

        #region Internal Constructors

        internal FloatingDamageInfo(IPlayer target, float damage, int currentMs, IModAPI modAPI, SettingsHandler settingsHandler, DxHandler dxHandler, TimerHandler timerHandler)
        {
            ModAPI = modAPI;
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;

            _damage = damage;
            _startTicks = currentMs;
            _targetPosition = target.Position;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Remove()
        {
            _text?.Remove();
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
            ModAPI.Graphics.GetScreenCoordFromWorldCoord(_targetPosition.X, _targetPosition.Y, _targetPosition.Z, ref screenX, ref screenY);

            float percentage = (float)elapsedTicks / _settingsHandler.PlayerSettings.ShowFloatingDamageInfoDurationMs;
            screenY -= 0.2f * percentage;

            float scale = 0.4f - (0.3f * percentage);
            var color = Color.FromArgb(255 - (int)(255 * percentage), 220, 220, 220);

            if (_text == null)
                _text = new DxText(_dxHandler, ModAPI, _timerHandler, _damage.ToString(), screenX, screenY, scale, color, alignmentX: AlignmentX.Center, alignmentY: AlignmentY.Bottom, dropShadow: false, outline: true);
            else
                _text.SetRelativeY(screenY);
        }

        #endregion Public Methods
    }
}
