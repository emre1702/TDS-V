using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Entities.GTA;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;

namespace TDS.Client.Handler.Lobby
{
    public class DamageTestMenuHandler
    {
        private bool _hasMenuOpened;
        private readonly BrowserHandler _browserHandler;

        public DamageTestMenuHandler(EventsHandler eventsHandler, BrowserHandler browserHandler)
        {
            _browserHandler = browserHandler;

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.WeaponChanged += EventsHandler_WeaponChanged;

            RAGE.Events.Add(ToClientEvent.ToggleDamageTestMenu, ToggleDamageTestMenu);
        }

        private void ToggleDamageTestMenu(object[] args)
        {
            _hasMenuOpened = (bool)args[0];
            if (!_hasMenuOpened)
            {
                _browserHandler.Angular.Browser.Call(ToBrowserEvent.ToggleDamageTestMenu, _hasMenuOpened);
                return;
            }

            var json = args[1].ToString();
            var weapon = ((TDSPlayer)RAGE.Elements.Player.LocalPlayer).CurrentWeapon;
            _browserHandler.Angular.Browser.Call(ToBrowserEvent.ToggleDamageTestMenu, _hasMenuOpened, json, ((uint)weapon).ToString());
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings _)
        {
            if (_hasMenuOpened)
            {
                _browserHandler.Angular.Browser.Call(ToBrowserEvent.ToggleDamageTestMenu, false);
                _hasMenuOpened = false;
            }
        }

        private void EventsHandler_WeaponChanged(WeaponHash previousWeapon, WeaponHash currentHash)
        {
            _browserHandler.Angular.ExecuteFast(ToBrowserEvent.SetDamageTestMenuCurrentWeapon, ((uint)currentHash).ToString());
        }
    }
}
