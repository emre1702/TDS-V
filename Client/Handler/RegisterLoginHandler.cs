using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.PlayerCommands;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class RegisterLoginHandler : ServiceBase
    {
        #region Private Fields

        private readonly BrowserHandler _browserHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly Serializer _serializer;
        private readonly SettingsHandler _settingsHandler;

        #endregion Private Fields

        #region Public Constructors

        public RegisterLoginHandler(IModAPI modAPI, LoggingHandler loggingHandler, CursorHandler cursorHandler, RemoteEventsSender remoteEventsSender,
            BrowserHandler browserHandler, SettingsHandler settingsHandler, Serializer serializer, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            _cursorHandler = cursorHandler;
            _remoteEventsSender = remoteEventsSender;
            _browserHandler = browserHandler;
            _settingsHandler = settingsHandler;
            _serializer = serializer;
            _eventsHandler = eventsHandler;

            modAPI.Event.Add(FromBrowserEvent.TryLogin, TryLogin);
            modAPI.Event.Add(FromBrowserEvent.TryRegister, TryRegister);
            modAPI.Event.Add(ToClientEvent.StartRegisterLogin, OnStartRegisterLoginMethod);
            modAPI.Event.Add(ToClientEvent.LoginSuccessful, OnLoginSuccessfulMethod);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Start(string name, bool isRegistered)
        {
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

        public void TryLogin(object[] args)
        {
            string username = (string)args[0];
            string password = (string)args[1];
            _remoteEventsSender.Send(ToServerEvent.TryLogin, username, SharedUtils.HashPWClient(password));
        }

        public void TryRegister(object[] args)
        {
            string username = (string)args[0];
            string password = (string)args[1];
            string email = (string)args[2];
            _remoteEventsSender.Send(ToServerEvent.TryRegister, username, SharedUtils.HashPWClient(password), email ?? string.Empty, (int)_settingsHandler.LanguageEnum);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnLoginSuccessfulMethod(object[] args)
        {
            Stop();
            _settingsHandler.LoadSyncedSettings(_serializer.FromServer<SyncedServerSettingsDto>(args[0].ToString()));
            _settingsHandler.LoadUserSettings(_serializer.FromServer<SyncedPlayerSettingsDto>(args[1].ToString()));
            _settingsHandler.LoggedIn = true;

            _browserHandler.Angular.SetReady((string)args[2]);

            _eventsHandler.OnLoggedIn();

            SendWelcomeMessage();
        }

        private void OnStartRegisterLoginMethod(object[] args)
        {
            string scname = (string)args[0];
            bool isregistered = Convert.ToBoolean(args[1]);
            Start(scname, isregistered);
        }

        private void SendWelcomeMessage()
        {
            ModAPI.Chat.Output("#o#__________________________________________");
            ModAPI.Chat.Output(string.Join("#n#", _settingsHandler.Language.WELCOME_MESSAGE));
            ModAPI.Chat.Output("#o#__________________________________________");
        }

        #endregion Private Methods
    }
}
