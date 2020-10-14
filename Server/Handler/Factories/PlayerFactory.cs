using TDS_Server.Data.Interfaces.PlayersSystem;

namespace TDS_Server.Handler.Factories
{
    public class PlayerFactory
    {
        public PlayerFactory(IPlayerProvider playerProvider)
        {
            RAGE.Entities.Players.CreateEntity = netHandle => playerProvider.Create(netHandle);
        }
    }
}
