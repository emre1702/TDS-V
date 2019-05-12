using Newtonsoft.Json;
using RAGE.Ui;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Manager.Utility;

namespace TDS_Client.Manager.Account
{
    internal static class RegisterLogin
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
            EventsSender.Send(DToServerEvent.TryRegister, CommonUtils.HashPWClient(password), email ?? string.Empty);
        }

        public static void Start(string theName, bool isregistered)
        {
            name = theName;
            isRegistered = isregistered;
            Browser = new HtmlWindow(Constants.RegisterLoginBrowserPath);
            CursorManager.Visible = true;
            SendDataToBrowser();
        }

        public static void Stop()
        {
            Browser?.Destroy();
            Browser = null;
            CursorManager.Visible = false;
        }

        private static void SendDataToBrowser()
        {
            Browser.ExecuteJs($"setLoginPanelData(`{name}`, {(isRegistered ? 1 : 0)}, `{JsonConvert.SerializeObject(Settings.Language.LOGIN_REGISTER_TEXTS)}`)");
        }

        public static void SyncLanguage()
        {
            Browser?.ExecuteJs($"loadLanguage(`{JsonConvert.SerializeObject(Settings.Language.LOGIN_REGISTER_TEXTS)}`)");
        }
    }
}