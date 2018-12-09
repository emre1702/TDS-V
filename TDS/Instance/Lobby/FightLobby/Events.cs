using GTANetworkAPI;
using TDS.Instance.Player;
using TDS.Instance.Utility;

namespace TDS.Instance.Lobby
{
    partial class FightLobby
    {
        public override void OnPlayerDeath(Character character, Client killer, uint weapon, bool spawnPlayer = true)
        {
            if (character.Team.IsSpectatorTeam)
            {
                SpectateNextAllTeams(character);
                return;
            }

            base.OnPlayerDeath(character, killer, weapon, false);

            // was alive //
            if (character.Lifes > 0)
            {
                DeathInfoSync(character.Player, character.Team.Id, killer, weapon);
                if (--character.Lifes > 0 && spawnPlayer)
                {
                    DeathSpawnTimer[character] = new Timer(() =>
                    {
                        NAPI.Player.SpawnPlayer(character.Player, spawnPoint.Around(LobbyEntity.AroundSpawnPoint), LobbyEntity.DefaultSpawnRotation);
                    }, LobbyEntity.SpawnAgainAfterDeathMs);
                }
                else if (character.Lifes == 0)
                {
                    DeathSpawnTimer[character] = new Timer(() =>
                    {
                        SpectateNextSameTeam(character);
                    }, LobbyEntity.SpawnAgainAfterDeathMs);
                }
            } 
        }

        public virtual void OnPlayerWeaponSwitch(Character character, WeaponHash oldweapon, WeaponHash newweapon)
        {
#warning todo Check if its needed
            //NAPI.ClientEvent.TriggerClientEvent(character.Player, "onClientPlayerWeaponChange", (int)newweapon);
        }
    }
}
