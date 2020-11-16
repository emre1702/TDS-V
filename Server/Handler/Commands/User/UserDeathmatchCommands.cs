using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Commands.User
{
    public class UserDeathmatchCommands
    {
        [TDSCommandAttribute(UserCommand.Suicide)]
        public void Suicide(ITDSPlayer player)
        {
            if (!(player.Lobby is IFightLobby fightLobby))
                return;
            if (player.Lifes == 0)
                return;

            var animName = "PILL";
            var animTime = 0.536f;
            switch (player.CurrentWeapon)
            {
                // Pistols //
                case WeaponHash.Pistol:
                case WeaponHash.Combatpistol:
                case WeaponHash.Appistol:
                case WeaponHash.Pistol50:
                case WeaponHash.Revolver:
                case WeaponHash.Snspistol:
                case WeaponHash.Heavypistol:
                case WeaponHash.Doubleaction:
                case WeaponHash.Revolver_mk2:
                case WeaponHash.Snspistol_mk2:
                case WeaponHash.Pistol_mk2:
                case WeaponHash.Vintagepistol:
                case WeaponHash.Marksmanpistol:
                    animName = "PISTOL";
                    animTime = 0.365f;
                    break;
            }

            NAPI.Task.RunSafe(() => fightLobby.Sync.TriggerEvent(ToClientEvent.ApplySuicideAnimation, player.RemoteId, animName, animTime));
        }

    }
}
