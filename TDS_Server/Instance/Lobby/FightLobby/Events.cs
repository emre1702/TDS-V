using GTANetworkAPI;
using TDS_Server.Default;
using TDS_Server.Instance.Player;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby
    {
        public override void OnPlayerDeath(TDSPlayer character, Client killer, uint weapon, bool spawnPlayer = true)
        {
            if (character.Team.Index == 0)
            {
                SpectateOtherAllTeams(character);
                return;
            }

            DmgSys.OnPlayerDeath(character, killer, weapon);
            base.OnPlayerDeath(character, killer, weapon, false);

            // was alive //
            if (character.Lifes > 0)
            {
                DeathInfoSync(character.Client, character.Team.Index, killer, weapon);

                if (--character.Lifes == 0 && spawnPlayer)
                {
                    DeathSpawnTimer[character] = new TDSTimer(() =>
                    {
                        SpectateOtherSameTeam(character);
                        NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerSpectateMode);
                    }, LobbyEntity.SpawnAgainAfterDeathMs.Value);
                }
            }
        }

        public virtual void OnPlayerWeaponSwitch(TDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.PlayerWeaponChange, newWeapon, DmgSys.GetDamage(newWeapon));
        }
    }
}
