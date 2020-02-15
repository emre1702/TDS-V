using RAGE.Elements;
using TDS_Common.Default;
using TDS_Common.Enum;

namespace TDS_Client.Manager.Utility
{
    static class Crouching
    {
        private const string _movementClipSet = "move_ped_crouched";
        private const string _strafeClipSet = "move_ped_crouched_strafing";
        private const float _clipSetSwitchTime = 0.25f;

        public static void Init()
        {
            // BindManager.Add(Enum.EKey.LCtrl, ToggleCrouch, Enum.EKeyPressState.Up);
            PlayerDataSync.OnDataChanged += PlayerDataSync_OnDataChanged;
        }

        public static void OnEntityStreamIn(Entity entity)
        {
            if (entity.Type != Type.Player)
                return;

            var player = (Player)entity;
            bool isCrouched = PlayerDataSync.GetData<bool>(player, EPlayerDataKey.Crouched);
            if (isCrouched)
            {
                player.SetMovementClipset(_movementClipSet, _clipSetSwitchTime);
                player.SetStrafeClipset(_strafeClipSet);
            }
        }

        private static void PlayerDataSync_OnDataChanged(Player player, EPlayerDataKey key, object data)
        {
            if (key != EPlayerDataKey.Crouched)
                return;
            if ((bool)data)
            {
                player.SetMovementClipset(_movementClipSet, _clipSetSwitchTime);
                player.SetStrafeClipset(_strafeClipSet);
            }
            else
            {
                player.ResetMovementClipset(_clipSetSwitchTime);
                player.ResetStrafeClipset();
            }
        }

        private static void ToggleCrouch(Enum.EKey _)
        {
            EventsSender.Send(DToServerEvent.ToggleCrouch);
        }
    }
}
