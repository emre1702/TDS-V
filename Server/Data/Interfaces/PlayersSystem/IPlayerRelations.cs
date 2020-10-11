using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerRelations
    {
        PlayerRelation GetRelationTo(ITDSPlayer target);

        bool HasRelationTo(ITDSPlayer target, PlayerRelation relation);

        void Init(ITDSPlayer player, IPlayerEvents events);

        void SetRelation(ITDSPlayer target, PlayerRelation relation);
    }
}
