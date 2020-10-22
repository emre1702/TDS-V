using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class RegisterLoginBrowserHandler : BrowserHandlerBase
    {
        public RegisterLoginBrowserHandler(LoggingHandler loggingHandler)
            : base(loggingHandler, Constants.RegisterLoginBrowserPath)
        { }

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
