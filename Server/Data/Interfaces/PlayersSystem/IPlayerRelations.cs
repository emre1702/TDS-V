using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerRelations
    {
        PlayerRelation GetRelationTo(ITDSPlayer target);

        bool HasRelationTo(ITDSPlayer target, PlayerRelation relation);

        void Init(ITDSPlayer player, IPlayerEvents events);

        void SetRelation(ITDSPlayer target, PlayerRelation relation);
    }
}
