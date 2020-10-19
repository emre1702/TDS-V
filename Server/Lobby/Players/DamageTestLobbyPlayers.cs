using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.Players
{
    public class DamageTestLobbyPlayers : FightLobbyPlayers
    {
        public DamageTestLobbyPlayers(IFightLobby lobby, IFightLobbyEventsHandler events) : base(lobby, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            if (!await base.AddPlayer(player, teamIndex))
                return false;

            NAPI.Task.Run(() =>
            {
                player.TriggerBrowserEvent(ToBrowserEvent.ToggleDamageTestMenu, true);
            });

            return true;
        }

        public override async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            if (!await base.RemovePlayer(player))
                return false;

            NAPI.Task.Run(() =>
            {
                player.TriggerBrowserEvent(ToBrowserEvent.ToggleDamageTestMenu, false);
            });

            return true;
        }
    }
}
