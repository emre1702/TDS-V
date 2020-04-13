using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Client.RAGEAPI.Player;
using TDS_Shared.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    public class WeaponShotHandler : BaseEventHandler<WeaponShotDelegate>
    {
        private readonly PlayerConvertingHandler _playerConvertingHandler;

        public WeaponShotHandler(PlayerConvertingHandler playerConvertingHandler) : base()
        {
            _playerConvertingHandler = playerConvertingHandler;

            RAGE.Events.OnPlayerWeaponShot += OnWeaponShot;
        }

        private void OnWeaponShot(RAGE.Vector3 targetPos, RAGE.Elements.Player modTarget, RAGE.Events.CancelEventArgs modCancel)
        {
            if (Actions.Count == 0)
                return;

            var pos = targetPos.ToPosition3D();
            var target = _playerConvertingHandler.GetPlayer(modTarget);
            var cancel = new CancelEventArgs();

            foreach (var action in Actions)
                if (action.Requirement is null || action.Requirement())
                    action.Method(pos, target, cancel);

            modCancel.Cancel = cancel.Cancel;
        }
     }
}
