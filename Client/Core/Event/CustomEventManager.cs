
using TDS_Client.Interface;
using TDS_Client.Manager.Utility;
using TDS_Shared.Dto;
using TDS_Shared.Enum;

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
        public static event EmptyDelegate OnMapClear;

        public delegate void RoundStartDelegate(bool isSpectator);
        public static event RoundStartDelegate OnRoundStart;


        public delegate void LanguageChangedDelegate(ILanguage newLang);
        public static event LanguageChangedDelegate OnLanguageChanged;



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

        public static void SetMapClear()
        {
            OnMapClear?.Invoke();
        }

        public static void SetNewLanguage(ILanguage newLang)
        {
            OnLanguageChanged?.Invoke(newLang);
        }

    }
}
