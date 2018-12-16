namespace TDS.Manager.Utility
{
    using System;
    using System.Collections.Generic;
	using GTANetworkAPI;
    using TDS.Interface;
    using TDS.Enum;
    using TDS.Instance.Language;
    using TDS.Manager.Player;
    using TDS.Instance.Player;

    static class LangUtils
	{
        private static readonly Dictionary<ELanguage, ILanguage> languageByID = new Dictionary<ELanguage, ILanguage>
        {
            [ELanguage.German] = new German(),
            [ELanguage.English] = new English()
        };

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

        /* public static void SendLangMessage(this Client player, Func<ILanguage, string> propertygetter)
         {
             NAPI.Chat.SendChatMessageToPlayer(player, propertygetter(player.GetLang()));
         }*/



        /*public static string GetLang(this Client player, string type, params string[] args)
		{
			return GetLang(player.GetChar(), type, args);
		}

		public static string GetLang(Language language, string type, params string[] args)
		{
			if ( args.Length == 0 )
				return langData[language][type];
			return GetReplaced(langData[language][type], args);
		}

		public static void SendLangMessage(this Character character, string type, params string[] args)
		{
			NAPI.Chat.SendChatMessageToPlayer(character.Player, character.GetLang(type, args));
		}

		public static void SendLangMessage(this Client player, string type, params string[] args)
		{
			NAPI.Chat.SendChatMessageToPlayer(player, player.GetLang(type, args));
		}

		public static void SendLangNotification(this Character character, string type, params string[] args)
		{
			NAPI.Notification.SendNotificationToPlayer(character.Player, character.GetLang(type, args));
		}

		public static void SendLangNotification(this Client player, string type, params string[] args)
		{
			NAPI.Notification.SendNotificationToPlayer(player, player.GetLang(type, args));
		}


		public static void SendMessageToAll(string type, params string[] args)
		{
			Dictionary<Language, string> texts = GetLangDictionary(type, args);
			List<Client> players = NAPI.Pools.GetAllPlayers();
			foreach ( Client player in players )
			{
				NAPI.Chat.SendChatMessageToPlayer(player, texts[player.GetChar().Language]);
			}
		}

		public static void SendNotificationToAll(string type, params string[] args)
		{
			Dictionary<Language, string> texts = GetLangDictionary(type, args);
			List<Client> players = NAPI.Pools.GetAllPlayers();
			foreach ( Client player in players )
			{
				NAPI.Notification.SendNotificationToPlayer(player, texts[player.GetChar().Language]);
			}
		}*/
    }

}
