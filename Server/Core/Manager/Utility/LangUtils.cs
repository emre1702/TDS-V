using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Shared.Data.Enums;
using TDS_Server.Instance.Language;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Interfaces;
using TDS_Server.Manager.PlayerManager;

namespace TDS_Server.Core.Manager.Utility
{
    internal static class LangUtils
    {
        public static readonly Dictionary<ELanguage, ILanguage> LanguageByID = new Dictionary<ELanguage, ILanguage>
        {
            [ELanguage.German] = new German(),
            [ELanguage.English] = new English()
        };

        public static void SendAllChatMessage(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in LanguageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in PlayerManager.PlayerManager.LoggedInPlayers)
            {
                player.SendMessage(returndict[player.Language]);
            }
        }

        public static void SendAllNotification(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in LanguageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in PlayerManager.PlayerManager.LoggedInPlayers)
            {
                player.SendNotification(returndict[player.Language]);
            }
        }

        public static ILanguage GetLang(this Player player)
        {
            return LanguageByID[player.GetChar().LanguageEnum];
        }

        public static ILanguage GetLang(this TDSPlayer character)
        {
            return LanguageByID[character.LanguageEnum];
        }

        public static ILanguage GetLang(this Type language)
        {
            foreach (ILanguage lang in LanguageByID.Values)
            {
                if (lang.GetType() == language.GetType())
                {
                    return lang;
                }
            }
            return LanguageByID[ELanguage.English];
        }

        public static ILanguage GetLang(byte language)
        {
            return LanguageByID[(ELanguage)language];
        }

        public static ILanguage GetLang(this ELanguage language)
        {
            return LanguageByID[language];
        }

        public static Dictionary<ILanguage, string> GetLangDictionary(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in LanguageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }
            return returndict;
        }
    }
}
