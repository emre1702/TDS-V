using RAGE;
using TDS_Client.Manager.Damage;
using static RAGE.Events;
using Player = RAGE.Elements.Player;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {
        private void AddRAGEEvents()
        {
            OnPlayerWeaponShot += OnPlayerWeaponShotMethod;
            OnPlayerSpawn += OnPlayerSpawnMethod;
            OnPlayerDeath += OnPlayerDeathMethod;
            OnPlayerQuit += OnPlayerQuitMethod;
        }

        private void OnPlayerWeaponShotMethod(Vector3 targetPos, Player target, CancelEventArgs cancel)
        {
            Damagesys.OnWeaponShot(targetPos, target, cancel);
        }

        private void OnPlayerSpawnMethod(CancelEventArgs cancel)
        {
            Death.PlayerSpawn();
        }

        private void OnPlayerDeathMethod(Player player, uint reason, Player killer, CancelEventArgs cancel)
        {
            Death.PlayerDeath(player);
        }

        private void OnPlayerQuitMethod(Player player)
        {
        }

    }
}
