﻿using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class RegisterLoginBrowserHandler : BrowserHandlerBase
    {
        public RegisterLoginBrowserHandler(IModAPI modAPI, LoggingHandler loggingHandler, Serializer serializer, SettingsHandler settingsHandler)
            : base(modAPI, loggingHandler, serializer, Constants.RegisterLoginBrowserPath)
        {
            modAPI.Event.Add(FromBrowserEvent.SyncRegisterLoginLanguageTexts, _ => SyncLanguage(settingsHandler.Language));
        }

        public void SendDataToBrowser(string name, bool isRegistered, ILanguage lang)
        {
            Browser.ExecuteJs($"setLoginPanelData(`{name}`, {(isRegistered ? 1 : 0)}, `{Serializer.ToBrowser(lang.LOGIN_REGISTER_TEXTS)}`)");
        }

        public void SyncLanguage(ILanguage lang)
        {
            Browser?.ExecuteJs($"loadLanguage(`{Serializer.ToBrowser(lang.LOGIN_REGISTER_TEXTS)}`)");
        }
    }
}
