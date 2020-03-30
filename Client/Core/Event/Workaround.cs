using TDS_Client.Manager.Utility;
using TDS_Shared.Default;
using static RAGE.Events;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {
        public static void AddWorkaroundEvents()
        {
            Add(ToClientEvent.AttachEntityToEntityWorkaround, Workaround.AttachEntityToEntityWorkaroundMethod);
            Add(ToClientEvent.DetachEntityWorkaround, Workaround.DetachEntityWorkaroundMethod);
            Add(ToClientEvent.FreezeEntityWorkaround, Workaround.FreezeEntityWorkaroundMethod);
            Add(ToClientEvent.FreezePlayerWorkaround, Workaround.FreezePlayerWorkaroundMethod);
            Add(ToClientEvent.SetEntityCollisionlessWorkaround, Workaround.SetEntityCollisionlessWorkaroundMethod);
            Add(ToClientEvent.SetEntityInvincible, Workaround.SetEntityInvincibleMethod);
            Add(ToClientEvent.SetPlayerInvincible, Workaround.SetPlayerInvincibleMethod);
            Add(ToClientEvent.SetPlayerTeamWorkaround, Workaround.SetPlayerTeamWorkaroundMethod);
        }
    }
}
