using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Instance.Utility;
using TDS_Server.Instance.Player;

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

                if (character.Lifes == 1 && spawnPlayer)
                {
                    DeathSpawnTimer[character] = new TDSTimer(() =>
                    {
                        SpectateOtherSameTeam(character);
                        NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerSpectateMode);
                    }, (uint)LobbyEntity.SpawnAgainAfterDeathMs);
                }
            }
            --character.Lifes;
        }

        public virtual void OnPlayerWeaponSwitch(TDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.PlayerWeaponChange, newWeapon, DmgSys.GetDamage((EWeaponHash)newWeapon));
        }
    }
}