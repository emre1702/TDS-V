using TDS.Server.Data.Interfaces.GangActionAreaSystem.GangsHandler;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Handler.GangSystem;

namespace TDS.Server.GangActionAreaSystem.GangsHandlers
{
    internal class BaseAreaGangsHandler : IBaseGangActionAreaGangsHandler
    {
        public IGang? Owner { get; private set; }
        public IGang? Attacker { get; private set; }

        private readonly GangsHandler _gangsHandler;

        public BaseAreaGangsHandler(GangsHandler gangsHandler)
        {
            _gangsHandler = gangsHandler;
        }

        public void Init(GangActionAreas entity)
        {
            if (entity.OwnerGangId != _gangsHandler.None.Entity.Id)
                Owner = _gangsHandler.GetById(entity.OwnerGangId);
        }

        public void SetAttacker(IGang? attacker)
            => Attacker = attacker;

        public void SetOwner(IGang? owner) 
            => Owner = owner;
    }
}
