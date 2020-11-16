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
        private bool _hasMenuOpened = true;
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
            var toggle = (bool)args[0];
            var json = args.Length > 1 ? (string)args[1] : null;
            var weapon = ((TDSPlayer)RAGE.Elements.Player.LocalPlayer).CurrentWeapon;
            _browserHandler.Angular.Browser.Call(ToBrowserEvent.ToggleDamageTestMenu, toggle, json, ((uint)weapon).ToString());
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
