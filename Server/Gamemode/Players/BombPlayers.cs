using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.GamemodesSystem.Players;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;

namespace TDS_Server.GamemodesSystem.Players
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
            await base.PlayerLeftAfter(data);
            if (Planter == data.Player)
                Planter = null;
            else if (BombAtPlayer == data.Player)
                _bombGamemode.Specials.DropBomb();
        }
    }
}
