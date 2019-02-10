using Newtonsoft.Json;
using RAGE;
using RAGE.Ui;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Manager.Utility;

namespace TDS_Client.Manager.Account
{
    static class RegisterLogin
    {
        public static HtmlWindow Browser;
        private static string name;
        private static bool isRegistered;

        public static void TryLogin(string password)
        {
            EventsSender.Send(DToServerEvent.TryLogin, CommonUtils.HashPWClient(password));
        }

        public static void TryRegister(string password, string email)
        {
            EventsSender.Send(DToServerEvent.TryRegister, CommonUtils.HashPWClient(password), email);
        }

        public static void Start(string theName, bool isregistered)
        {
            name = theName;
            isRegistered = isregistered;
            Browser = new HtmlWindow(Constants.RegisterLoginBrowserPath);
            CursorManager.Visible = true;
        }

        public static void Stop()
        {
            Browser.Destroy();
            Browser = null;
            CursorManager.Visible = false;
        }

        public static void SendDataToBrowser()
        {
            Browser.ExecuteJs($"setLoginPanelData(`{name}`, {isRegistered}, `{JsonConvert.SerializeObject(Settings.Language.LOGIN_REGISTER_TEXTS)}`)");
        }

        public static void SyncLanguage()
        {
            Browser.ExecuteJs($"loadLanguage(`{JsonConvert.SerializeObject(Settings.Language.LOGIN_REGISTER_TEXTS)}`)");
        }
    }
}
