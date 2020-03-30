using System;

namespace TDS_Client.Handler.Events
{
    public class EventsHandler
    {
        public delegate void BoolDelegate(bool visible);
        public event BoolDelegate CursorToggled;

        internal void OnCursorToggled(bool visible)
        {
            CursorToggled?.Invoke(visible);
        }
    }
}
