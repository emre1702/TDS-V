using RAGE.Ui;
using TDS_Client.Manager.Utility;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;

namespace TDS_Client.Manager.Account
{
    internal static class RegisterLogin
    {
        public static HtmlWindow Browser;
        private static string _name;
        private static bool _isRegistered;

        public static void TryLogin(string username, string password)
        {
            EventsSender.Send(DToServerEvent.TryLogin, username, SharedUtils.HashPWClient(password));
        }

        public static void TryRegister(string username, string password, string email)
        {
            EventsSender.Send(DToServerEvent.TryRegister, username, SharedUtils.HashPWClient(password), email ?? string.Empty);
        }

        public static void Start(string theName, bool isregistered)
        {
            _name = theName;
            _isRegistered = isregistered;
            Browser = new HtmlWindow(ClientConstants.RegisterLoginBrowserPath);
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
            Browser.ExecuteJs($"setLoginPanelData(`{_name}`, {(_isRegistered ? 1 : 0)}, `{Serializer.ToBrowser(Settings.Language.LOGIN_REGISTER_TEXTS)}`)");
        }

        public static void SyncLanguage()
        {
            Browser?.ExecuteJs($"loadLanguage(`{Serializer.ToBrowser(Settings.Language.LOGIN_REGISTER_TEXTS)}`)");
        }
    }
}
