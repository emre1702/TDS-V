using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerMoneyHandler
    {
        int Money { get; set; }

        void Init(ITDSPlayer player);

        void GiveMoney(int money);

        void GiveMoney(uint money);
    }
}
