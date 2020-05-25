using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.RAGEAPI.Player
{
    internal class PlayerAPI : IPlayerAPI
    {
        #region Public Methods

        public void SetHealth(IPlayer player, int health)
        {
            NAPI.Player.SetPlayerHealth(player as Player, health);
        }

        #endregion Public Methods
    }
}
