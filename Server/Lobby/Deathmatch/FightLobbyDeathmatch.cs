using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class FightLobbyDeathmatch : BaseLobbyDeathmatch
    {
        public FightLobbyDeathmatch(IBaseLobbyEventsHandler events, FightLobby fightLobby)
            : base(events, fightLobby)
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
