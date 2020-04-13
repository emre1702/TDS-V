using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Shared.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    class PlayerSpawnEventHandler : BaseEventHandler<SpawnDelegate>
    {
        public PlayerSpawnEventHandler() : base()
        {
            RAGE.Events.OnPlayerSpawn += PlayerSpawn;
        }

        private void PlayerSpawn(RAGE.Events.CancelEventArgs cancelMod)
        {
            if (Actions.Count == 0)
                return;

            var cancel = new CancelEventArgs();

            foreach (var action in Actions)
                if (action.Requirement is null || action.Requirement())
                    action.Method(cancel);

            cancelMod.Cancel = cancel.Cancel;
        }
    }
}
