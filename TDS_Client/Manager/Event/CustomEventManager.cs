
using TDS_Client.Interface;
using TDS_Client.Manager.Utility;
using TDS_Common.Dto;
using TDS_Common.Enum;

namespace TDS_Client.Manager.Event
{
    static class CustomEventManager
    {
        public delegate void LobbySwitchDelegate(SyncedLobbySettingsDto settings);
        public static event LobbySwitchDelegate OnLobbyLeave;
        public static event LobbySwitchDelegate OnLobbyJoin;

        public delegate void EmptyDelegate();
        public static event EmptyDelegate OnDeath;
        public static event EmptyDelegate OnRoundEnd;

        public delegate void RoundStartDelegate(bool isSpectator);
        public static event RoundStartDelegate OnRoundStart;

        public delegate void WeaponChangeDelegate(uint oldWeaponHash, uint newWeaponHash);
        public static event WeaponChangeDelegate OnWeaponChange;

        public delegate void LanguageChangedDelegate(ILanguage newLang);
        public static event LanguageChangedDelegate OnLanguageChanged;

        private static uint _lastWeaponHash = 0;

        public static void SetLobbyLeave(SyncedLobbySettingsDto settings)
        {
            //OnPlayerLoggedInBefore?.Invoke(player);
            OnLobbyLeave?.Invoke(settings);
        }

        public static void SetLobbyJoin(SyncedLobbySettingsDto settings)
        {
            //OnPlayerLoggedOutBefore?.Invoke(player);
            OnLobbyJoin?.Invoke(settings);
        }

        public static void SetDead()
        {
            OnDeath?.Invoke();
        }

        public static void SetRoundStart(bool isSpectator)
        {
            OnRoundStart?.Invoke(isSpectator);
        }

        public static void SetRoundEnd()
        {
            OnRoundEnd?.Invoke();
        }

        public static void SetNewWeapon(uint newWeapon)
        {
            if (newWeapon == _lastWeaponHash)
                return;

            OnWeaponChange?.Invoke(_lastWeaponHash, newWeapon);
            _lastWeaponHash = newWeapon;
        }

        public static void SetNewLanguage(ILanguage newLang)
        {
            OnLanguageChanged?.Invoke(newLang);
        }
    }
}
