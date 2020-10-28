using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Players
{
    public class DamageTestLobbyPlayers : FightLobbyPlayers
    {
        private string? _damagesToSync;

        protected new IDamageTestLobby Lobby => (IDamageTestLobby)base.Lobby;

        public DamageTestLobbyPlayers(IDamageTestLobby lobby, IFightLobbyEventsHandler events) : base(lobby, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            if (!await base.AddPlayer(player, teamIndex))
                return false;
            var isOwner = Lobby.Players.IsLobbyOwner(player);
            player.HealthAndArmor.DisableDying = true;

            // load damage datas
            if (_damagesToSync is null)
                _damagesToSync = Serializer.ToBrowser(Lobby.Deathmatch.GetWeaponDamages());

            NAPI.Task.RunSafe(() =>
            {
                player.TriggerEvent(ToClientEvent.PlayerRespawned);
                if (isOwner)
                    player.TriggerEvent(ToClientEvent.ToggleDamageTestMenu, true, _damagesToSync);
            });

            return true;
        }

        public override async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            if (!await base.RemovePlayer(player))
                return false;
            player.HealthAndArmor.DisableDying = false;

            return true;
        }
    }
}
