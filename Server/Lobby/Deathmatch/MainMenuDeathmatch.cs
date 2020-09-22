using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class MainMenuDeathmatch : BaseLobbyDeathmatch
    {
        public MainMenuDeathmatch(IBaseLobbyEventsHandler events, MainMenu lobby) : base(events, lobby)
        {
        }

        public override void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            base.OnPlayerDeath(player, killer, weapon);

            player.Spawn(Lobby.MapHandler.SpawnPoint, Lobby.MapHandler.SpawnRotation);
            player.Freeze(true);
        }
    }
}
