using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Manager.Utility;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Manager.Browser.Angular
{
    public class RegisterLoginBrowserHandler : BrowserHandlerBase
    {
        public RegisterLoginBrowserHandler(IModAPI modAPI, Serializer serializer)
            : base(modAPI, serializer, Constants.RegisterLoginBrowserPath)
        {    }

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
