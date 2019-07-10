using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Common.Enum;
using TDS_Server.Instance.Language;
using TDS_Server.Instance.Player;
using TDS_Server.Interface;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Utility
{
    internal static class LangUtils
    {
        private static readonly Dictionary<ELanguage, ILanguage> _languageByID = new Dictionary<ELanguage, ILanguage>
        {
            [ELanguage.German] = new German(),
            [ELanguage.English] = new English()
        };

        public static void SendAllChatMessage(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in _languageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in Player.Player.LoggedInPlayers)
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client, returndict[player.Language]);
            }
        }

        public static void SendAllNotification(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in _languageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in Player.Player.LoggedInPlayers)
            {
                NAPI.Notification.SendNotificationToPlayer(player.Client, returndict[player.Language]);
            }
        }

        public static ILanguage GetLang(this Client player)
        {
            return _languageByID[player.GetChar().LanguageEnum];
        }

        public static ILanguage GetLang(this TDSPlayer character)
        {
            return _languageByID[character.LanguageEnum];
        }

        public static ILanguage GetLang(this Type language)
        {
            foreach (ILanguage lang in _languageByID.Values)
            {
                if (lang.GetType() == language.GetType())
                {
                    return lang;
                }
            }
            return _languageByID[ELanguage.English];
        }

        public static ILanguage GetLang(byte language)
        {
            return _languageByID[(ELanguage)language];
        }

        public static ILanguage GetLang(this ELanguage language)
        {
            return _languageByID[language];
        }

        public static Dictionary<ILanguage, string> GetLangDictionary(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in _languageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }
            return returndict;
        }
    }
}