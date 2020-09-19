using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class RegisterLoginHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        private readonly SettingsHandler _settingsHandler;

        public RegisterLoginHandler(LoggingHandler loggingHandler, CursorHandler cursorHandler, RemoteEventsSender remoteEventsSender,
            BrowserHandler browserHandler, SettingsHandler settingsHandler, EventsHandler eventsHandler)
            : base(loggingHandler)
        {
            _cursorHandler = cursorHandler;
            _remoteEventsSender = remoteEventsSender;
            _browserHandler = browserHandler;
            _settingsHandler = settingsHandler;

            _eventsHandler = eventsHandler;

            RAGE.Events.Add(FromBrowserEvent.TryLogin, TryLogin);
            RAGE.Events.Add(FromBrowserEvent.TryRegister, TryRegister);
            RAGE.Events.Add(ToClientEvent.StartRegisterLogin, OnStartRegisterLoginMethod);
            RAGE.Events.Add(ToClientEvent.LoginSuccessful, OnLoginSuccessfulMethod);
        }

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
            RAGE.Chat.Show(true);
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

        private void OnLoginSuccessfulMethod(object[] args)
        {
            Stop();
            _settingsHandler.LoadSyncedSettings(Serializer.FromServer<SyncedServerSettingsDto>(args[0].ToString()));
            _settingsHandler.LoadUserSettings(Serializer.FromServer<SyncedPlayerSettingsDto>(args[1].ToString()));
            _settingsHandler.LoadThemeSettings(Serializer.FromServer<SyncedPlayerThemeSettings>(args[2].ToString()));
            _settingsHandler.LoggedIn = true;

            _browserHandler.Angular.SetReady((string)args[3]);

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
            RAGE.Chat.Output("#o#__________________________________________");
            RAGE.Chat.Output(string.Join("#n#", _settingsHandler.Language.WELCOME_MESSAGE));
            RAGE.Chat.Output("#o#__________________________________________");
        }
    }
}