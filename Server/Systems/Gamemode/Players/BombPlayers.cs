using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.GamemodesSystem.Players;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;

namespace TDS.Server.GamemodesSystem.Players
{
    public class BombPlayers : BaseGamemodePlayers, IBombGamemodePlayers
    {
        public ITDSPlayer? Planter { get; set; }
        public ITDSPlayer? BombAtPlayer { get; set; }

        private readonly IBombGamemode _bombGamemode;

        public BombPlayers(IBombGamemode bombGamemode)
        {
            _bombGamemode = bombGamemode;
        }

        protected override async ValueTask PlayerLeftAfter((ITDSPlayer Player, int HadLifes) data)
        {
            await base.PlayerLeftAfter(data).ConfigureAwait(false);
            if (Planter == data.Player)
                Planter = null;
            else if (BombAtPlayer == data.Player)
                _bombGamemode.Specials.DropBomb();
        }
    }
}
