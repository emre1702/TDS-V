using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        private Dictionary<int, PlayerRelation> _relationsToUsersDict = new Dictionary<int, PlayerRelation>();

        private void LoadRelations()
        {
            if (_entity is null)
                return;

            _relationsToUsersDict = _entity.PlayerRelationsPlayer.ToDictionary(r => r.TargetId, r => r.Relation);
        }

        public void SetRelation(ITDSPlayer target, PlayerRelation relation)
        {
            _relationsToUsersDict[target.Id] = relation;
        }

        public PlayerRelation GetRelationTo(ITDSPlayer target)
        {
            if (_relationsToUsersDict.TryGetValue(target.Id, out PlayerRelation relation))
                return relation;

            return PlayerRelation.None;
        }

        public bool HasRelationTo(ITDSPlayer target, PlayerRelation relation)
            => GetRelationTo(target) == relation;
    }
}
