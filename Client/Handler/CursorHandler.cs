using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Events;

namespace TDS_Client.Handler
{
    public class CursorHandler
    {
        public bool Visible
        {
            get => _modAPI.Cursor.Visible;
            set
            {
                if (value)
                {
                    if (++_cursorOpenedCounter == 1)
                    {
                        _modAPI.Cursor.Visible = true;
                        _eventsHandler.OnCursorToggled(true);
                    }
                }
                else if (--_cursorOpenedCounter <= 0)
                {
                    _modAPI.Cursor.Visible = false;
                    _cursorOpenedCounter = 0;
                    _eventsHandler.OnCursorToggled(false);
                }
            }
        }

        private int _cursorOpenedCounter;

        private readonly IModAPI _modAPI;
        private readonly EventsHandler _eventsHandler;

        public CursorHandler(IModAPI modAPI, EventsHandler eventsHandler, BindsHandler bindsHandler)
        {
            _modAPI = modAPI;
            _eventsHandler = eventsHandler;

            bindsHandler.Add(Key.End, ManuallyToggleCursor);
        }

        public void ManuallyToggleCursor(Key _)
        {
            bool isVisible = _modAPI.Cursor.Visible;
            _cursorOpenedCounter = isVisible ? 0 : 1;
            _modAPI.Cursor.Visible = !isVisible;
            _eventsHandler.OnCursorToggled(!isVisible);
        }
    }
}
