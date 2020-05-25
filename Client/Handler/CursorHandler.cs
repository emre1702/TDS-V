using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Events;

namespace TDS_Client.Handler
{
    public class CursorHandler : ServiceBase
    {
        #region Private Fields

        private readonly EventsHandler _eventsHandler;

        private readonly SettingsHandler _settingsHandler;

        private int _cursorOpenedCounter;

        #endregion Private Fields

        #region Public Constructors

        public CursorHandler(IModAPI modAPI, LoggingHandler loggingHandler, EventsHandler eventsHandler, BindsHandler bindsHandler, SettingsHandler settingsHandler)
            : base(modAPI, loggingHandler)
        {
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;

            eventsHandler.ChatInputToggled += EventsHandler_ChatInputToggled;
            eventsHandler.CursorToggleRequested += b => Visible = b;

            bindsHandler.Add(Key.End, ManuallyToggleCursor);
        }

        #endregion Public Constructors

        #region Public Properties

        public bool Visible
        {
            get => ModAPI.Cursor.Visible;
            set
            {
                if (value)
                {
                    if (++_cursorOpenedCounter == 1)
                    {
                        ModAPI.Cursor.Visible = true;
                        _eventsHandler.OnCursorToggled(true);
                    }
                }
                else if (--_cursorOpenedCounter <= 0)
                {
                    ModAPI.Cursor.Visible = false;
                    _cursorOpenedCounter = 0;
                    _eventsHandler.OnCursorToggled(false);
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void ManuallyToggleCursor(Key _)
        {
            bool isVisible = ModAPI.Cursor.Visible;
            _cursorOpenedCounter = isVisible ? 0 : 1;
            ModAPI.Cursor.Visible = !isVisible;
            _eventsHandler.OnCursorToggled(!isVisible);
        }

        #endregion Public Methods

        #region Private Methods

        private void EventsHandler_ChatInputToggled(bool boolean)
        {
            if (_settingsHandler.PlayerSettings.ShowCursorOnChatOpen)
                Visible = boolean;
        }

        #endregion Private Methods
    }
}
