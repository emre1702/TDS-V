using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using static RAGE.Events;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {
        public static void AddWorkaroundEvents()
        {
            Add(DToClientEvent.AttachEntityToEntityWorkaround, Workaround.AttachEntityToEntityWorkaroundMethod);
            Add(DToClientEvent.DetachEntityWorkaround, Workaround.DetachEntityWorkaroundMethod);
            Add(DToClientEvent.FreezePlayerWorkaround, Workaround.FreezePlayerWorkaroundMethod);
            Add(DToClientEvent.SetEntityCollisionlessWorkaround, Workaround.SetEntityCollisionlessWorkaroundMethod);
            Add(DToClientEvent.SetEntityInvincible, Workaround.SetEntityInvincibleMethod);
            Add(DToClientEvent.SetPlayerInvincible, Workaround.SetPlayerInvincibleMethod);
            Add(DToClientEvent.SetPlayerTeamWorkaround, Workaround.SetPlayerTeamWorkaroundMethod);
        }
    }
}
