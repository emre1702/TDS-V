using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Notifications;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Handler;

namespace TDS.Server.GangActionAreaSystem.Notifications
{
    internal class BaseAreaNotifications : IBaseGangActionAreaNotifications
    {
        private readonly LobbiesHandler _lobbiesHandler;

#nullable disable
        private IBaseGangActionArea _area;
#nullable enable

        internal BaseAreaNotifications(LobbiesHandler lobbiesHandler)
            => _lobbiesHandler = lobbiesHandler;

        public void Init(IBaseGangActionArea area)
        {
            _area = area;

            area.Events.Conquered += OnConquered;
        }

        private void OnConquered(IGang newOwner, IGang? previousOwner)
        {
            if (previousOwner is { })
                _lobbiesHandler.GangLobby.Notifications.Send(lang =>
                    string.Format(lang.GANG_AREA_CONQUERED_WITH_OWNER, newOwner.Entity.Name, previousOwner.Entity.Name, _area.Type));
            else
                _lobbiesHandler.GangLobby.Notifications.Send(lang =>
                    string.Format(lang.GANG_AREA_CONQUERED_WITHOUT_OWNER, newOwner.Entity.Name, _area.Type));
        }
    }
}
