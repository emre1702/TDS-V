using System.Collections.Generic;
using System.Linq;

namespace TDS_Client.RAGEAPI.Pool
{
    internal class PoolPlayersAPI : IPoolPlayersAPI
    {
        #region Public Constructors

        public PoolPlayersAPI()
        {
            RAGE.Elements.Entities.Players.CreateEntity = (ushort id, ushort remoteId) => new Player.Player(id, remoteId);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<ITDSPlayer> All => RAGE.Elements.Entities.Players.All.OfType<ITDSPlayer>().ToList();

        public List<ITDSPlayer> Streamed => RAGE.Elements.Entities.Players.Streamed.OfType<ITDSPlayer>().ToList();

        #endregion Public Properties

        #region Public Methods

        public ITDSPlayer GetAt(ushort id)
            => RAGE.Elements.Entities.Players.GetAt(id) as ITDSPlayer;

        public ITDSPlayer GetAtHandle(int handle)
                    => RAGE.Elements.Entities.Players.GetAtHandle(handle) as ITDSPlayer;

        public ITDSPlayer GetAtRemote(ushort handleValue)
            => RAGE.Elements.Entities.Players.GetAtRemote(handleValue) as ITDSPlayer;

        #endregion Public Methods
    }
}
