using TDS_Client.Manager.Utility;
using TDS_Shared.Default;
using static RAGE.Events;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {
        public static void AddWorkaroundEvents()
        {
            Add(ToClientEvent.AttachEntityToEntityWorkaround, WorkaroundsHandler.AttachEntityToEntityWorkaroundMethod);
            Add(ToClientEvent.DetachEntityWorkaround, WorkaroundsHandler.DetachEntityWorkaroundMethod);
            Add(ToClientEvent.FreezeEntityWorkaround, WorkaroundsHandler.FreezeEntityWorkaroundMethod);
            Add(ToClientEvent.FreezePlayerWorkaround, WorkaroundsHandler.FreezePlayerWorkaroundMethod);
            Add(ToClientEvent.SetEntityCollisionlessWorkaround, WorkaroundsHandler.SetEntityCollisionlessWorkaroundMethod);
            Add(ToClientEvent.SetEntityInvincible, WorkaroundsHandler.SetEntityInvincibleMethod);
            Add(ToClientEvent.SetPlayerInvincible, WorkaroundsHandler.SetPlayerInvincibleMethod);
            Add(ToClientEvent.SetPlayerTeamWorkaround, WorkaroundsHandler.SetPlayerTeamWorkaroundMethod);
        }
    }
}
