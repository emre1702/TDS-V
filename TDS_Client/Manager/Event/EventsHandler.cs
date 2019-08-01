using TDS_Client.Instance.Draw.Dx;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {
        public EventsHandler()
        {
            LoadOnStart();
            AddRAGEEvents();
            AddFromServerEvents();
            AddFromBrowserEvents();
            AddWorkaroundEvents();
        }

        private void LoadOnStart()
        {
            Dx.RefreshResolution();
        }
    }
}