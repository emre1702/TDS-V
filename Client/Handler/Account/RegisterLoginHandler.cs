using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Browser;
using TDS_Client.Handler.Browser;
using TDS_Client.Manager.Utility;
using TDS_Shared.Core;
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

        public RegisterLoginHandler(CursorHandler cursorHandler, RemoteEventsSender remoteEventsSender, BrowserHandler browserHandler) 
            => (_cursorHandler, _remoteEventsSender, _browserHandler) = (cursorHandler, remoteEventsSender, browserHandler);

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
            _browserHandler.RegisterLogin.SendDataToBrowser(name, isRegistered);
        }

        public void Stop()
        {
            _browserHandler.RegisterLogin.Stop();
            _cursorHandler.Visible = false;
        }        
    }
}
