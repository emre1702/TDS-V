﻿using System.Collections.Generic;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Core.Damagesystem
{
    partial class Damagesys
    {
        private static readonly Dictionary<ITDSPlayer, ITDSPlayer> sDeadTimer = new Dictionary<ITDSPlayer, ITDSPlayer>();

        public void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            if (player.ModPlayer is null)
                return;
            if (sDeadTimer.ContainsKey(player))
                return;
            player.ModPlayer.Freeze(true);

            KillingSpreeDeath(player);

            if (player.Lifes <= 0)
                return;

            // Death //
            if (player.LobbyStats != null && player.Lobby?.SavePlayerLobbyStats == true)
            {
                ++player.LobbyStats.Deaths;
                ++player.LobbyStats.TotalDeaths;
            }

            // Kill //
            if (killer != player)
            {
                if (killer.CurrentRoundStats != null)
                {
                    ++killer.CurrentRoundStats.Kills;
                }

                KillingSpreeKill(killer);
            }

            // Assist //
            CheckForAssist(player, killer);

            if (player.Lobby?.SavePlayerLobbyStats == true && player.Lobby?.IsOfficial == true)
            {
                _loggingHandler.LogKill(player, killer, weapon);
            }

        }

        public ITDSPlayer GetKiller(ITDSPlayer player, ITDSPlayer? possiblekiller)
        {
            // It's the killer from the Death event //
            if (player != possiblekiller && possiblekiller != null)
                return possiblekiller;

            // It's the last hitter //
            if (player.LastHitter != null)
                return player.LastHitter;

            // It's a suicide //
            return player;
        }

        private void CheckForAssist(ITDSPlayer player, ITDSPlayer killer)
        {
            if (!_allHitters.ContainsKey(player))
                return;

            int halfarmorhp = player.Lobby!.StartTotalHP / 2;
            foreach (KeyValuePair<ITDSPlayer, int> entry in _allHitters[player])
            {
                if (entry.Value >= halfarmorhp)
                {
                    ITDSPlayer possibleAssister = entry.Key;
                    if (possibleAssister.ModPlayer is { } && possibleAssister.Lobby == player.Lobby && possibleAssister != killer && possibleAssister.CurrentRoundStats is { })
                    {
                        ++possibleAssister.CurrentRoundStats.Assists;
                        possibleAssister.SendNotification(string.Format(possibleAssister.Language.GOT_ASSIST, player.DisplayName));
                    }
                    if (killer != possibleAssister ||
                        halfarmorhp % 2 != 0 ||
                        entry.Value != halfarmorhp / 2 ||
                        _allHitters[player].Count > 2)
                        break;
                }
            }
        }

        public void CheckLastHitter(ITDSPlayer character, out ITDSPlayer? killer)
        {
            killer = null;
            if (character.LastHitter is null)
                return;

            ITDSPlayer lastHitterCharacter = character.LastHitter;
            character.LastHitter = null;

            if (lastHitterCharacter.ModPlayer is null)
                return;

            if (character.Lobby != lastHitterCharacter.Lobby)
                return;

            if (lastHitterCharacter.Lifes == 0)
                return;

            if (lastHitterCharacter.CurrentRoundStats != null)
                ++lastHitterCharacter.CurrentRoundStats.Kills;
            KillingSpreeKill(lastHitterCharacter);
            lastHitterCharacter.SendNotification(string.Format(lastHitterCharacter.Language.GOT_LAST_HITTED_KILL, character.DisplayName));
            killer = lastHitterCharacter;
        }
    }
}