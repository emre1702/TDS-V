using System;
using TDS.Client.Data.Defaults;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Models;
using TDS.Shared.Data.Utility;
using TDS.Shared.Default;

namespace TDS.Client.Handler
{
    public class RegisterLoginHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        private readonly SettingsHandler _settingsHandler;
        private readonly CursorHandler _cursorHandler;

        public RegisterLoginHandler(LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender,
            BrowserHandler browserHandler, SettingsHandler settingsHandler, EventsHandler eventsHandler, CursorHandler cursorHandler)
            : base(loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _browserHandler = browserHandler;
            _settingsHandler = settingsHandler;
            _cursorHandler = cursorHandler;

            _eventsHandler = eventsHandler;

            eventsHandler.AngularBrowserCreated += Started;

            RAGE.Events.Add(FromBrowserEvent.TryLogin, TryLogin);
            RAGE.Events.Add(FromBrowserEvent.TryRegister, TryRegister);
            RAGE.Events.Add(FromBrowserEvent.ResetPassword, ResetPassword);
            RAGE.Events.Add(ToClientEvent.LoginSuccessful, OnLoginSuccessfulMethod);
        }

        public void Started()
        {
            RAGE.Chat.Show(false);
            _cursorHandler.Visible = true;
        }

        public void Stopped()
        {
            RAGE.Chat.Show(true);
            _cursorHandler.Visible = false;
        }

        public void TryLogin(object[] args)
        {
            string username = (string)args[0];
            string password = (string)args[1];
            if (!_remoteEventsSender.Send(ToServerEvent.TryLogin, username, SharedUtils.HashPWClient(password)))
                _browserHandler.Angular.ExecuteFast(FromBrowserEvent.TryLogin, _settingsHandler.Language.COOLDOWN);
        }

        public void TryRegister(object[] args)
        {
            string username = (string)args[0];
            string password = (string)args[1];
            string email = (string)args[2];
            _remoteEventsSender.Send(ToServerEvent.TryRegister, username, SharedUtils.HashPWClient(password), email ?? string.Empty, (int)_settingsHandler.LanguageEnum);
        }

        public void ResetPassword(object[] args)
        {
            string username = (string)args[0];
            string email = (string)args[1];
            _remoteEventsSender.Send(ToServerEvent.ResetPassword, username, email);
        }

        private void OnLoginSuccessfulMethod(object[] args)
        {
            Stopped();
            _settingsHandler.LoadSyncedSettings(Serializer.FromServer<SyncedServerSettingsDto>(args[0].ToString()));
            _settingsHandler.LoggedIn = true;

            _browserHandler.Angular.SetReady((string)args[1]);

            _eventsHandler.OnLoggedIn();

            SendWelcomeMessage();
        }

        private void SendWelcomeMessage()
        {
            RAGE.Chat.Output("#o#__________________________________________");
            RAGE.Chat.Output(string.Join("#n#", _settingsHandler.Language.WELCOME_MESSAGE));
            RAGE.Chat.Output("#o#__________________________________________");
        }
    }
}