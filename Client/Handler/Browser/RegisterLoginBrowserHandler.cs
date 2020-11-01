using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class RegisterLoginBrowserHandler : BrowserHandlerBase
    {
        private string _cachedName;
        private int _cachedRegistered;
        private string _cachedLangJson;

        public RegisterLoginBrowserHandler(LoggingHandler loggingHandler)
            : base(loggingHandler, Constants.RegisterLoginBrowserPath)
        { }

        public override void CreateBrowser()
        {
            base.CreateBrowser();
            Browser.ExecuteJs($"mp.trigger('{FromBrowserEvent.Created}', 'RegisterLogin')");
        }

        public void SendDataToBrowser(string name, bool isRegistered, ILanguage lang)
        {
            _cachedName = name;
            _cachedRegistered = isRegistered ? 1 : 0;
            _cachedLangJson = Serializer.ToBrowser(lang.LOGIN_REGISTER_TEXTS);

            SetLoginPanelData();
        }

        public void SetLoginPanelData()
        {
            if (_cachedName is null)
                return;
            Browser.ExecuteJs($"setLoginPanelData(`{_cachedName}`, {_cachedRegistered}, `{_cachedLangJson}`)");
        }

        public void SyncLanguage(ILanguage lang)
        {
            _cachedLangJson = Serializer.ToBrowser(lang.LOGIN_REGISTER_TEXTS);
            Browser?.ExecuteJs($"loadLanguage(`{_cachedLangJson}`)");
        }
    }
}
