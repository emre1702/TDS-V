﻿using GTANetworkAPI;
using TDS_Server.Default;
using TDS_Server.Instance.Player;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby
    {
        public override void OnPlayerDeath(TDSPlayer character, Client possibleKillerClient, uint weapon, bool spawnPlayer = true)
        {
            if (character.Team == null || character.Team.IsSpectator)
            {
                SpectateOtherAllTeams(character);
                return;
            }

            TDSPlayer? killer = DmgSys.OnPlayerDeath(character, possibleKillerClient, weapon);
            base.OnPlayerDeath(character, possibleKillerClient, weapon, false);

            // was alive //
            if (character.Lifes > 0)
            {
                DeathInfoSync(character, killer, weapon);

                if (--character.Lifes == 0 && spawnPlayer)
                {
                    DeathSpawnTimer[character] = new TDSTimer(() =>
                    {
                        SpectateOtherSameTeam(character);
                        NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerSpectateMode);
                    }, LobbyEntity.SpawnAgainAfterDeathMs ?? 50);
                }
            }
        }

        public virtual void OnPlayerWeaponSwitch(TDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.PlayerWeaponChange, newWeapon, DmgSys.GetDamage(newWeapon));
        }
    }
}
