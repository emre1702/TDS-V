namespace TDS_Server.Manager.Utility
{
    using System;
    using System.Collections.Generic;
	using GTANetworkAPI;
    using TDS_Server.Interface;
    using TDS_Server.Instance.Language;
    using TDS_Server.Manager.Player;
    using TDS_Server.Instance.Player;
    using TDS_Common.Enum;

    static class LangUtils
	{
        private static readonly Dictionary<ELanguage, ILanguage> languageByID = new Dictionary<ELanguage, ILanguage>
        {
            [ELanguage.German] = new German(),
            [ELanguage.English] = new English()
        };

        public static void SendAllChatMessage(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in languageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }
            
            foreach (Client client in NAPI.Pools.GetAllPlayers())
            {
                TDSPlayer player = client.GetChar();
                if (player.LoggedIn)
                    NAPI.Chat.SendChatMessageToPlayer(client, returndict[player.Language]);
            }
        }

        public static ILanguage GetLang (this Client player)
        {
            return languageByID[player.GetChar().LanguageEnum];
        }

		public static ILanguage GetLang (this TDSPlayer character)
		{
            return languageByID[character.LanguageEnum];
        }

        public static ILanguage GetLang (this Type language)
        {
            foreach (ILanguage lang in languageByID.Values)
            {
                if (lang.GetType() == language.GetType())
                {
                    return lang;
                }
            }
            return languageByID[ELanguage.English];
        }

        public static ILanguage GetLang(byte language)
        {
            return languageByID[(ELanguage)language];
        }

        public static ILanguage GetLang(this ELanguage language)
        {
            return languageByID[language];
        }

        public static Dictionary<ILanguage, string> GetLangDictionary (Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in languageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }
            return returndict;
        }
    }
}
