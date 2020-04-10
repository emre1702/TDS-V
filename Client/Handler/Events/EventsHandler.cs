using System;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Handler.Events
{
    public class EventsHandler
    {

        public delegate void BoolDelegate(bool boolean);
        public event BoolDelegate CursorToggled;
        public event BoolDelegate InFightStatusChanged;

        public delegate void EmptyDelegate();
        public event EmptyDelegate SettingsLoaded;

        public delegate void LanguageChangedDelegate(ILanguage lang, bool beforeLogin);
        public event LanguageChangedDelegate LanguageChanged;

        public delegate void WeaponChangedDelegate(WeaponHash previousWeapon, WeaponHash currentHash);
        public event WeaponChangedDelegate WeaponChanged;

        public delegate void DataChangedDelegate(IPlayer player, PlayerDataKey key, object data);
        public event DataChangedDelegate DataChanged;

        private WeaponHash _lastWeaponHash;

        private readonly IModAPI _modAPI;

        public EventsHandler(IModAPI modAPI)
        {
            _modAPI = modAPI;

            _modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnTick));
        }

        internal void OnCursorToggled(bool visible)
        {
            CursorToggled?.Invoke(visible);
        }

        internal void OnLanguageChanged(ILanguage lang, bool beforeLogin)
        {
            LanguageChanged?.Invoke(lang, beforeLogin);
        }

        internal void OnSettingsLoaded()
        {
            SettingsLoaded?.Invoke();
        }

        internal void OnInFightStatusChanged(bool value)
        {
            InFightStatusChanged?.Invoke(value);
        }

        internal void OnDataChanged(IPlayer player, PlayerDataKey key, object data)
        {
            DataChanged?.Invoke(player, key, data);
        }
        


        private void OnTick(ulong _)
        {
            CheckNewWeapon();
        }

        private void CheckNewWeapon()
        {
            var currentWeapon = _modAPI.LocalPlayer.GetSelectedWeapon();
            if (currentWeapon != _lastWeaponHash)
            {
                WeaponChanged?.Invoke(_lastWeaponHash, currentWeapon);
                _lastWeaponHash = currentWeapon;
            }
        }
    }
}
