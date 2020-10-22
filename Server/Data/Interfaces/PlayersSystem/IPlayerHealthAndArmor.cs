using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerHealthAndArmor
    {
        int Armor { get; set; }
        int Health { get; set; }
        bool DisableDying { get; set; }

        void Add(int effectiveHp, out int effectiveHpAdded);

        void Init(ITDSPlayer player);

        void Remove(int effectiveHp, out int effectiveHpRemoved, out bool killed);
    }
}
