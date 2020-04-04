using System;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.Events
{
    public class EventsHandler
    {

        public delegate void BoolDelegate(bool visible);
        public event BoolDelegate CursorToggled;

        public delegate void EmptyDelegate();

        internal void OnCursorToggled(bool visible)
        {
            CursorToggled?.Invoke(visible);
        }

    }
}
