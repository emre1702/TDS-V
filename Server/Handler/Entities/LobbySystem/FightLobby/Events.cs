using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class FightLobby
    {
        public override void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon, bool spawnPlayer = true)
        {
            if (player.Team is null || player.Team.IsSpectator)
            {
                SpectateOtherAllTeams(player);
                return;
            }

            DmgSys.OnPlayerDeath(player, killer, weapon);
            base.OnPlayerDeath(player, killer, weapon, false);

            // was alive //
            if (player.Lifes > 0)
            {
                DeathInfoSync(player, killer, weapon);

                if (player.Lifes == 1 && spawnPlayer)
                {
                    DeathSpawnTimer[player] = new TDSTimer(() =>
                    {
                        SpectateOtherSameTeam(player);
                        player.SendEvent(ToClientEvent.PlayerSpectateMode);
                    }, (uint)Entity.FightSettings.SpawnAgainAfterDeathMs);
                }
                --player.Lifes;
            }
            // Bug occured, he had 0 life and died
            // Spawn him again so the camera effects for wasted (and blackscreen) disappears
            else
            {
                DeathSpawnTimer[player] = new TDSTimer(() =>
                {
                    SpectateOtherSameTeam(player);
                    player.SendEvent(ToClientEvent.PlayerSpectateMode);
                }, (uint)Entity.FightSettings.SpawnAgainAfterDeathMs);
            }
            
        }

        public virtual void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            // NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.PlayerWeaponChange, (uint)newWeapon /*, DmgSys.GetDamage((WeaponHash)newWeapon)*/);
        }
    }
}
