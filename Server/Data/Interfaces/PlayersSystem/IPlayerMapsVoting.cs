using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerMapsVoting
    {
        void CheckReduceMapBoughtCounter();

        void Init(ITDSPlayer player);

        void SetBoughtMap(int price);
    }
}
