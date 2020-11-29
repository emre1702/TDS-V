using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Data.Interfaces.GangActionAreaSystem.GangsHandler
{
#nullable enable
    public interface IBaseGangActionAreaGangsHandler
    {
        IGang? Attacker { get; }
        IGang? Owner { get; }

        void Init(GangActionAreas entity);
        void SetAttacker(IGang? attacker);
        void SetOwner(IGang? owner);
    }
}