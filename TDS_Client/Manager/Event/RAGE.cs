﻿using RAGE;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Utility;
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
    }
}
