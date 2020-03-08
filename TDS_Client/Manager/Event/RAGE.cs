using System;
using RAGE;
using RAGE.Elements;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using static RAGE.Events;
using Player = RAGE.Elements.Player;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Event
{
    partial class EventsHandler : Script
    {
        private void AddRAGEEvents()
        {
            OnPlayerSpawn += OnPlayerSpawnMethod;
            OnPlayerDeath += OnPlayerDeathMethod;
            OnPlayerStartTalking += OnPlayerStartTalkingMethod;
            OnPlayerStopTalking += OnPlayerStopTalkingMethod;
            OnPlayerWeaponShot += OnPlayerWeaponShotMethod;
            OnEntityStreamIn += OnEntityStreamInMethod;
            OnIncomingDamage += OnIncomingDamageMethod;
            OnOutgoingDamage += OnOutgoingDamageMethod;
        }

        private void OnPlayerSpawnMethod(CancelEventArgs cancel)
        {
            Death.PlayerSpawn();
        }

        private void OnPlayerDeathMethod(Player player, uint reason, Player killer, CancelEventArgs cancel)
        {
            Death.PlayerDeath(player);
        }

        private void OnPlayerStartTalkingMethod(Player player)
        {
            MainBrowser.StartPlayerTalking(player.GetDisplayName());
        }

        private void OnPlayerStopTalkingMethod(Player player)
        {
            MainBrowser.StopPlayerTalking(player.GetDisplayName());
        }

        private void OnPlayerWeaponShotMethod(Vector3 targetPos, Player target, CancelEventArgs cancel)
        {
            AFKCheckManager.OnShoot();
        }

        private void OnEntityStreamInMethod(Entity entity)
        {
            Crouching.OnEntityStreamIn(entity);
        }

        private void OnIncomingDamageMethod(Player sourcePlayer, Entity sourceEntity, Entity targetEntity, ulong weaponHash, ulong boneIdx, int damage, CancelEventArgs cancel)
        {
            RAGE.Ui.Console.Log(RAGE.Ui.ConsoleVerbosity.Info, $"Incoming damage: Source {sourcePlayer.Name}, source entity {sourceEntity.Type.ToString()}, targetEntity {targetEntity.Type} - {targetEntity is Player}", true);

            MainBrowser.ShowBloodscreen();

            if (sourcePlayer != null)
            {
                cancel.Cancel = true;
                EventsSender.SendIgnoreCooldown(DToServerEvent.GotHit, (int)sourcePlayer.RemoteId, weaponHash.ToString(), boneIdx.ToString());
            }
            
        }


        private void OnOutgoingDamageMethod(Entity sourceEntity, Entity targetEntity, Player sourcePlayer, ulong weaponHash, ulong boneIdx, int damage, CancelEventArgs cancel)
        {
            RAGE.Ui.Console.Log(RAGE.Ui.ConsoleVerbosity.Info, $"Outgoing damage: Source {sourcePlayer.Name}, source entity {sourceEntity.Type.ToString()}, targetEntity {targetEntity.Type} - {targetEntity is Player}", true);

            FightInfo.HittedOpponent();
        }
    }
}
