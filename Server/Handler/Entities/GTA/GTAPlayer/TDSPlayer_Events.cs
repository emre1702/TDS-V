using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        public override void TriggerBrowserEvent(string eventName, params object[] args)
        {
            object[] newArgs = new object[args.Length + 1];
            newArgs[0] = eventName;
            for (int i = 0; i < args.Length; ++i)
            {
                newArgs[i + 1] = args[i];
            }
            TriggerEvent(ToClientEvent.ToBrowserEvent, newArgs);
        }
    }
}
