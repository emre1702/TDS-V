using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class GangLobbyDeathmatch : BaseLobbyDeathmatch
    {
        public GangLobbyDeathmatch(IBaseLobbyEventsHandler events, IBaseLobby lobby) : base(events, lobby)
        {
        }

        public override void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            base.OnPlayerDeath(player, killer, weapon);

            player.Spawn(player.Position, player.Rotation.Z);
        }
    }
}
