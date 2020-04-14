using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI.Player;

namespace TDS_Client.RAGEAPI.Player
{
    public class PlayerConvertingHandler
    {
        private Dictionary<RAGE.Elements.Player, IPlayer> _playersCache = new Dictionary<RAGE.Elements.Player, IPlayer>();

        public IPlayer GetPlayer(RAGE.Elements.Player modPlayer)
        {
            if (modPlayer is null)
                return null;

            if (!_playersCache.TryGetValue(modPlayer, out IPlayer player))
            {
                player = new Player(modPlayer);
                _playersCache.Add(modPlayer, player);
            }
            return player;
        }
    }
}
