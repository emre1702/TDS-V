using System;
using System.Collections.Generic;
using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Languages;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Helper
{
    public class LangHelper
    {
        private readonly Dictionary<Language, ILanguage> _languageById = new Dictionary<Language, ILanguage>
        {
            [Language.German] = new German(),
            [Language.English] = new English()
        };

        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        public LangHelper(ITDSPlayerHandler tdsPlayerHandler)
            => _tdsPlayerHandler = tdsPlayerHandler;

        public ILanguage GetLang(Type language)
        {
            lock (_languageById)
            {
                foreach (ILanguage lang in _languageById.Values)
                    if (lang.GetType() == language.GetType())
                        return lang;

                return _languageById[Language.English];
            }
        }

        public ILanguage GetLang(byte language)
        {
            lock (_languageById)
            {
                return _languageById[(Language)language];
            }
        }

        public ILanguage GetLang(Language language)
        {
            lock (_languageById)
            {
                return _languageById[language];
            }
        }

        public Dictionary<ILanguage, string> GetLangDictionary(Func<ILanguage, string> langGetter)
        {
            var returnDict = new Dictionary<ILanguage, string>();
            lock (_languageById)
            {
                foreach (ILanguage lang in _languageById.Values)
                    returnDict[lang] = langGetter(lang);
            }
            
            return returnDict;
        }

        public void SendAllChatMessage(Func<ILanguage, string> langGetter)
        {
            var langDictionary = GetLangDictionary(langGetter);

            NAPI.Task.RunSafe(() =>
            {
                foreach (var player in _tdsPlayerHandler.LoggedInPlayers)
                    player.SendChatMessage(langDictionary[player.Language]);
            });
        }

        public void SendAllNotification(Func<ILanguage, string> langGetter)
        {
            var langDictionary = GetLangDictionary(langGetter);

            NAPI.Task.RunSafe(() =>
            { 
                foreach (var player in _tdsPlayerHandler.LoggedInPlayers)
                    player.SendNotification(langDictionary[player.Language]);
            });
        }
    }
}
