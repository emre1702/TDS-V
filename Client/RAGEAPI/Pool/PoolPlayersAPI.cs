using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

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

        public List<IPlayer> All => RAGE.Elements.Entities.Players.All.OfType<IPlayer>().ToList();

        public List<IPlayer> Streamed => RAGE.Elements.Entities.Players.Streamed.OfType<IPlayer>().ToList();

        #endregion Public Properties

        #region Public Methods

        public IPlayer GetAt(ushort id)
            => RAGE.Elements.Entities.Players.GetAt(id) as IPlayer;

        public IPlayer GetAtHandle(int handle)
                    => RAGE.Elements.Entities.Players.GetAtHandle(handle) as IPlayer;

        public IPlayer GetAtRemote(ushort handleValue)
            => RAGE.Elements.Entities.Players.GetAtRemote(handleValue) as IPlayer;

        #endregion Public Methods
    }
}
