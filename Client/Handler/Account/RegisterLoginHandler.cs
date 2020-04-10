using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Account
{
    public class RegisterLoginHandler
    {
        private string _name;
        private bool _isRegistered;

        private readonly CursorHandler _cursorHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly BrowserHandler _browserHandler;
        private readonly SettingsHandler _settingsHandler;

        public RegisterLoginHandler(CursorHandler cursorHandler, RemoteEventsSender remoteEventsSender, BrowserHandler browserHandler, SettingsHandler settingsHandler)
            => (_cursorHandler, _remoteEventsSender, _browserHandler, _settingsHandler) = (cursorHandler, remoteEventsSender, browserHandler, settingsHandler);

        public void TryLogin(string username, string password)
        {
            _remoteEventsSender.Send(ToServerEvent.TryLogin, username, SharedUtils.HashPWClient(password));
        }

        public void TryRegister(string username, string password, string email)
        {
            _remoteEventsSender.Send(ToServerEvent.TryRegister, username, SharedUtils.HashPWClient(password), email ?? string.Empty);
        }

        public void Start(string name, bool isRegistered)
        {
            _name = name;
            _isRegistered = isRegistered;
            _browserHandler.RegisterLogin.CreateBrowser();
            //_browserHandler.RegisterLogin.SetReady(); only for Angular browser

            _cursorHandler.Visible = true;
            _browserHandler.RegisterLogin.SendDataToBrowser(name, isRegistered, _settingsHandler.Language);
        }

        public void Stop()
        {
            _browserHandler.RegisterLogin.Stop();
            _cursorHandler.Visible = false;
        }
    }
}
