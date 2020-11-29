using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGangActionHandler
    {
        int AttackCount { get; }
        bool InAction { get; }

        bool CheckCanAttack(ITDSPlayer outputTo);
        void SetActionEnded();
        void SetInAction(bool asAttacker);
    }
}