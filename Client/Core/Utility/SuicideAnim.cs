using PlayerElement = RAGE.Elements.Player;
using RAGE.Game;
using TDS_Shared.Default;

namespace TDS_Client.Manager.Utility
{
    static class SuicideAnim
    {
        private static bool _shotFired;
        private static string _animName;
        private static float _animTime;

        public static void ApplyAnimation(PlayerElement player, string animName, float animTime)
        {
            player.TaskPlayAnim("MP_SUICIDE", animName, 8f, 0, -1, 0, 0, false, false, false);

            if (player.RemoteId != PlayerElement.LocalPlayer.RemoteId)
                return;

            _animName = animName;
            _animTime = animTime;
            _shotFired = false;
            TickManager.Remove(OnRender);
            TickManager.Add(OnRender);
        }

        private static void OnRender()
        {
            if (!PlayerElement.LocalPlayer.IsPlayingAnim("MP_SUICIDE", _animName, 3))
            {
                TickManager.Remove(OnRender);
                return;
            }

            if (_animName == "PISTOL" && !_shotFired && PlayerElement.LocalPlayer.HasAnimEventFired(Misc.GetHashKey("Fire")))
            {
                _shotFired = true;
                EventsSender.Send(ToServerEvent.SuicideShoot);
            }

            if (PlayerElement.LocalPlayer.GetAnimCurrentTime("MP_SUICIDE", _animName) >= _animTime)
            {
                TickManager.Remove(OnRender);
                EventsSender.Send(ToServerEvent.SuicideKill);
            }
        }
    }
}
