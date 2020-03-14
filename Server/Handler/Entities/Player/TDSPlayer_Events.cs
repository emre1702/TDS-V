using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        public void SendBrowserEvent(string eventName, params object[] args)
        {
            object[] newArgs = new object[args.Length + 1];
            newArgs[0] = eventName;
            for (int i = 0; i < args.Length; ++i)
            {
                newArgs[i+1] = args[i];
            }
            ModPlayer?.SendEvent(ToClientEvent.ToBrowserEvent, newArgs);
        }

        public void SendEvent(string eventName, params object[] args)
        {
            ModPlayer?.SendEvent(eventName, args);
        }
    }
}
