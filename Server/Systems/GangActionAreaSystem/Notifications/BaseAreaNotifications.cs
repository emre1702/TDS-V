using BonusBotConnector.Client;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Notifications;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler;
using TDS.Server.Handler.Helper;

namespace TDS.Server.GangActionAreaSystem.Notifications
{
    internal class BaseAreaNotifications : IBaseGangActionAreaNotifications
    {
        private readonly BonusBotConnectorClient _bonusBotConnectorClient;
        private readonly LangHelper _langHelper;

#nullable disable
        private IBaseGangActionArea _area;
#nullable enable

        internal BaseAreaNotifications(BonusBotConnectorClient bonusBotConnectorClient, LangHelper langHelper)
            => (_bonusBotConnectorClient, _langHelper) = (bonusBotConnectorClient, langHelper);

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
            _langHelper.SendAllNotification(lang =>
                string.Format(lang.GANG_ACTION_IN_PREPARATION, _area));

            _area.GangsHandler.Attacker!.Chat.SendNotification(lang =>
                string.Format(lang.GANG_ACTION_IN_PREPARATION_ATTACKER, _area.GangsHandler.Owner, _area));

            _area.GangsHandler.Owner!.Chat.SendNotification(lang =>
                string.Format(lang.GANG_ACTION_IN_PREPARATION_OWNER, _area));
        }

        private ValueTask OnAttackStarted()
        {
            _area.InLobby!.Entity.Name = $"[GW] {_area.Attacker!.Entity.NameShort} - {_area.Owner!.Entity.NameShort}";

            _langHelper.SendAllNotification(lang =>
                string.Format(lang.GANG_ACTION_STARTED, _area.GangsHandler.Attacker, _area.GangsHandler.Owner, _area));
            _bonusBotConnectorClient.ChannelChat?.SendActionStartInfo(_area);
            return default;
        }

        private void OnConquered(IGang newOwner, IGang? previousOwner)
        {
            if (previousOwner is { })
                _langHelper.SendAllNotification(lang =>
                    string.Format(lang.GANG_AREA_CONQUERED_WITH_OWNER, _area, newOwner, previousOwner));
            else
                _langHelper.SendAllNotification(lang =>
                    string.Format(lang.GANG_AREA_CONQUERED_WITHOUT_OWNER, _area, newOwner));
        }

        private void OnDefended(IGang attacker, IGang owner) 
        {
            _langHelper.SendAllNotification(lang =>
                string.Format(lang.GANG_AREA_DEFENDED, _area, owner, attacker));
        }
    }
}
