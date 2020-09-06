using RAGE;
using RAGE.Elements;
using TDS_Client.Handler.Events;
using TDS_Shared.Default;
using static RAGE.Events;

namespace TDS_Client.Handler
{
    public class WeaponStatsHandler : ServiceBase
    {
        private readonly RemoteEventsSender _remoteEventsSender;

        public WeaponStatsHandler(LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender)
            : base(loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            OnPlayerWeaponShot += WeaponShot;
        }

        private void WeaponShot(Vector3 pos, Player target, CancelEventArgs cancelEventArgs)
        {
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.WeaponShot);
        }
    }
}
