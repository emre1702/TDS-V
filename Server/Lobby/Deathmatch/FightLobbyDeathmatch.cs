using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.LobbySystem.EventsHandlers;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class FightLobbyDeathmatch : BaseLobbyDeathmatch
    {
        public FightLobbyDeathmatch(BaseLobbyEventsHandler events) : base(events)
        {
        }

        protected override void ResetPlayer(ITDSPlayer player)
        {
            NAPI.Task.RunCustom(() =>
            {
                player.RemoveAllWeapons();
            });
        }
    }
}
