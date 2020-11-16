using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.GamemodesSystem.Gamemodes;

namespace TDS.Server.GamemodesSystem.Deathmatch
{
    public class BombDeathmatch : BaseGamemodeDeathmatch
    {
        private readonly BombGamemode _gamemode;

        public BombDeathmatch(BombGamemode gamemode)
        {
            _gamemode = gamemode;
        }

        internal override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);
            events.PlayerDied += PlayerDied;
        }

        internal override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);
            events.PlayerDied -= PlayerDied;
        }

        private void PlayerDied(ITDSPlayer player, ITDSPlayer killer, uint weapon, int hadLifes)
        {
            if (_gamemode.Players.BombAtPlayer == player)
                _gamemode.Specials.DropBomb();
        }
    }
}
