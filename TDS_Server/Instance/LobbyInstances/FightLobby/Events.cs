using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Instance.Utility;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class FightLobby
    {
        public override void OnPlayerDeath(TDSPlayer character, TDSPlayer killer, uint weapon, bool spawnPlayer = true)
        {
            if (character.Team is null || character.Team.IsSpectator)
            {
                SpectateOtherAllTeams(character);
                return;
            }

            DmgSys.OnPlayerDeath(character, killer, weapon);
            base.OnPlayerDeath(character, killer, weapon, false);

            // was alive //
            if (character.Lifes > 0)
            {
                DeathInfoSync(character, killer, weapon);

                if (character.Lifes == 1 && spawnPlayer)
                {
                    DeathSpawnTimer[character] = new TDSTimer(() =>
                    {
                        SpectateOtherSameTeam(character);
                        NAPI.ClientEvent.TriggerClientEvent(character.Player, DToClientEvent.PlayerSpectateMode);
                    }, (uint)LobbyEntity.SpawnAgainAfterDeathMs);
                }
            }
            --character.Lifes;
        }

        public virtual void OnPlayerWeaponSwitch(TDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.PlayerWeaponChange, (uint)newWeapon /*, DmgSys.GetDamage((EWeaponHash)newWeapon)*/);
        }
    }
}