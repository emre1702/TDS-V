using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Notifications;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
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

            area.Events.AddedToLobby += OnAddedToLobby;
            area.Events.RemovedFromLobby += OnRemovedFromLobby;
            area.Events.Conquered += OnConquered;
            area.Events.Defended += OnDefended;
        }

        private void OnAddedToLobby(IGangActionLobby lobby)
        {
            lobby.Events.Countdown += OnAttackPreparation;
            lobby.Events.InRound += OnAttackStarted;
        }

        private void OnRemovedFromLobby(IGangActionLobby lobby)
        {
            lobby.Events.Countdown -= OnAttackPreparation;
            if (lobby.Events.InRound is { })
                lobby.Events.InRound -= OnAttackStarted;
        }

        private void OnAttackPreparation()
        {
            _lobbiesHandler.GangLobby.Notifications.Send(lang =>
                string.Format(lang.GANG_ACTION_IN_PREPARATION, _area));

            _area.GangsHandler.Attacker!.Chat.SendNotification(lang =>
                string.Format(lang.GANG_ACTION_IN_PREPARATION_ATTACKER, _area.GangsHandler.Owner, _area));

            _area.GangsHandler.Owner!.Chat.SendNotification(lang =>
                string.Format(lang.GANG_ACTION_IN_PREPARATION_OWNER, _area));
        }

        private ValueTask OnAttackStarted()
        {
            _lobbiesHandler.GangLobby.Notifications.Send(lang =>
                string.Format(lang.GANG_ACTION_STARTED, _area.GangsHandler.Attacker, _area.GangsHandler.Owner, _area));
            return default;
        }

        private void OnConquered(IGang newOwner, IGang? previousOwner)
        {
            if (previousOwner is { })
                _lobbiesHandler.GangLobby.Notifications.Send(lang =>
                    string.Format(lang.GANG_AREA_CONQUERED_WITH_OWNER, _area, newOwner, previousOwner));
            else
                _lobbiesHandler.GangLobby.Notifications.Send(lang =>
                    string.Format(lang.GANG_AREA_CONQUERED_WITHOUT_OWNER, _area, newOwner));
        }

        private void OnDefended(IGang attacker, IGang owner) 
        {
            _lobbiesHandler.GangLobby.Notifications.Send(lang =>
                string.Format(lang.GANG_AREA_DEFENDED, _area, owner, attacker));
        }
    }
}
