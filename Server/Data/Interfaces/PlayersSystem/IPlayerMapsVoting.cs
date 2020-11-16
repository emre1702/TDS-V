using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerMapsVoting
    {
        void CheckReduceMapBoughtCounter();

        void Init(ITDSPlayer player);

        void SetBoughtMap(int price);
    }
}
