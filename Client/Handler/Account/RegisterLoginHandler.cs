using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Browser;
using TDS_Client.Manager.Utility;
using TDS_Shared.Core;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Account
{
    public class RegisterLoginHandler
    {
        public IBrowser Browser;
        private string _name;
        private bool _isRegistered;

        private readonly IModAPI _modAPI;
        private readonly CursorHandler _cursorHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;

        public RegisterLoginHandler(IModAPI modAPI, CursorHandler cursorHandler, RemoteEventsSender remoteEventsSender, SettingsHandler settingsHandler) 
            => (_modAPI, _cursorHandler, _settingsHandler) = (modAPI, cursorHandler, settingsHandler);

        public void TryLogin(string username, string password)
        {
            _remoteEventsSender.Send(ToServerEvent.TryLogin, username, SharedUtils.HashPWClient(password));
        }

        public void TryRegister(string username, string password, string email)
        {
            _remoteEventsSender.Send(ToServerEvent.TryRegister, username, SharedUtils.HashPWClient(password), email ?? string.Empty);
        }

        public void Start(string theName, bool isregistered)
        {
            _name = theName;
            _isRegistered = isregistered;
            Browser = _modAPI.Browser.Create(Constants.RegisterLoginBrowserPath);
            _cursorHandler.Visible = true;
            SendDataToBrowser();
        }

        public void Stop()
        {
            Browser?.Destroy();
            Browser = null;
            _cursorHandler.Visible = false;
        }

        private void SendDataToBrowser()
        {
            Browser.ExecuteJs($"setLoginPanelData(`{_name}`, {(_isRegistered ? 1 : 0)}, `{Serializer.ToBrowser(_settingsHandler.Language.LOGIN_REGISTER_TEXTS)}`)");
        }

        public void SyncLanguage()
        {
            Browser?.ExecuteJs($"loadLanguage(`{Serializer.ToBrowser(_settingsHandler.Language.LOGIN_REGISTER_TEXTS)}`)");
        }
    }
}
