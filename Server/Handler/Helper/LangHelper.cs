using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Languages;
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

        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        public LangHelper(ITDSPlayerHandler tdsPlayerHandler)
            => _tdsPlayerHandler = tdsPlayerHandler;

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
            var returnDict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in LanguageByID.Values)
            {
                returnDict[lang] = langgetter(lang);
            }
            return returnDict;
        }

        public void SendAllChatMessage(Func<ILanguage, string> langgetter)
        {
            var returnDict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in LanguageByID.Values)
            {
                returnDict[lang] = langgetter(lang);
            }

            foreach (var player in _tdsPlayerHandler.LoggedInPlayers)
            {
                player.SendChatMessage(returnDict[player.Language]);
            }
        }

        public void SendAllNotification(Func<ILanguage, string> langgetter)
        {
            var returnDict = new Dictionary<ILanguage, string>();
            foreach (ILanguage lang in LanguageByID.Values)
            {
                returnDict[lang] = langgetter(lang);
            }

            foreach (var player in _tdsPlayerHandler.LoggedInPlayers)
            {
                player.SendNotification(returnDict[player.Language]);
            }
        }
    }
}
