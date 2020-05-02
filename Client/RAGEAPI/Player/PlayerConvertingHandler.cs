using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Player
{
    public class PlayerConvertingHandler
    {
        private Dictionary<RAGE.Elements.Player, IPlayer> _playersCache = new Dictionary<RAGE.Elements.Player, IPlayer>();
        private readonly ILocalPlayer _localPlayer;
        private readonly EntityConvertingHandler _entityConvertingHandler;

        public PlayerConvertingHandler(RAGE.Elements.Player localPlayer, EntityConvertingHandler entityConvertingHandler)
        {
            _entityConvertingHandler = entityConvertingHandler;

            _localPlayer = new LocalPlayer(localPlayer, _entityConvertingHandler);
            _playersCache[localPlayer] = _localPlayer;
        }

        public IPlayer GetPlayer(RAGE.Elements.Player modPlayer)
        {
            if (modPlayer is null)
                return null;

            if (!_playersCache.TryGetValue(modPlayer, out IPlayer player))
            {
                player = new Player(modPlayer, _entityConvertingHandler);
                _playersCache.Add(modPlayer, player);
            }

            return player;
        }

        public ILocalPlayer GetLocalPlayer()
            => _localPlayer;
    }
}
