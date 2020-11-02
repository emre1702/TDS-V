using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Commands.User
{
    public class UserDeathmatchCommands
    {
        [TDSCommand(UserCommand.Suicide)]
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
