using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Language;
using TDS_Server.Handler.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Helper
{
    public class LangHelper
    {
        public readonly Dictionary<Language, ILanguage> LanguageByID = new Dictionary<Language, ILanguage>
        {
            [Language.German] = new German(),
            [Language.English] = new English()
        };

        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public LangHelper(TDSPlayerHandler tdsPlayerHandler)
            => _tdsPlayerHandler = tdsPlayerHandler;

        public void SendAllChatMessage(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in LanguageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in _tdsPlayerHandler.LoggedInPlayers)
            {
                player.SendMessage(returndict[player.Language]);
            }
        }

        public void SendAllNotification(Func<ILanguage, string> langgetter)
        {
            Dictionary<ILanguage, string> returndict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in LanguageByID.Values)
            {
                returndict[lang] = langgetter(lang);
            }

            foreach (var player in _tdsPlayerHandler.LoggedInPlayers)
            {
                player.SendNotification(returndict[player.Language]);
            }
        }

        public ILanguage GetLang(Type language)
        {
            foreach (ILanguage lang in LanguageByID.Values)
            {
                if (lang.GetType() == language.GetType())
                {
                    return lang;
                }
            }
            return LanguageByID[Language.English];
        }

        public ILanguage GetLang(byte language)
        {
            return LanguageByID[(Language)language];
        }

        public ILanguage GetLang(Language language)
        {
            return LanguageByID[language];
        }

        public Dictionary<ILanguage, string> GetLangDictionary(Func<ILanguage, string> langgetter)
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
