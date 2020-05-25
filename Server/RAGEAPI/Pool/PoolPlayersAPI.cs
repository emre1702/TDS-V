using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolPlayersAPI : IPoolPlayersAPI
    {
        #region Public Constructors

        public PoolPlayersAPI()
        {
            RAGE.Entities.Players.CreateEntity = netHandle => new Player.Player(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IPlayer> All => RAGE.Entities.Players.All.OfType<IPlayer>().ToList();
        public IEnumerable<IPlayer> AsEnumerable => RAGE.Entities.Players.AsEnumerable.OfType<IPlayer>();
        public int Count => RAGE.Entities.Players.Count;

        #endregion Public Properties

        #region Public Methods

        public IPlayer? GetAt(ushort id)
            => RAGE.Entities.Players.GetAt(id) as IPlayer;

        #endregion Public Methods
    }
}
