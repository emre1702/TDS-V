using System;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Client.RAGEAPI.Player;
using TDS_Shared.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    public class WeaponShotHandler : BaseEventHandler<WeaponShotDelegate>
    {
        #region Private Fields

        private readonly LoggingHandler _loggingHandler;
        private readonly PlayerConvertingHandler _playerConvertingHandler;

        #endregion Private Fields

        #region Public Constructors

        public WeaponShotHandler(LoggingHandler loggingHandler, PlayerConvertingHandler playerConvertingHandler) : base()
        {
            _loggingHandler = loggingHandler;
            _playerConvertingHandler = playerConvertingHandler;

            RAGE.Events.OnPlayerWeaponShot += OnWeaponShot;
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnWeaponShot(RAGE.Vector3 targetPos, RAGE.Elements.Player modTarget, RAGE.Events.CancelEventArgs modCancel)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                var pos = targetPos.ToPosition3D();
                var target = _playerConvertingHandler.GetPlayer(modTarget);
                var cancel = new CancelEventArgs();

                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(pos, target, cancel);
                }

                modCancel.Cancel = cancel.Cancel;
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        #endregion Private Methods
    }
}
