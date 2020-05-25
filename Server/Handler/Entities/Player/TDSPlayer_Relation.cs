using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        #region Private Fields

        private Dictionary<int, PlayerRelation> _relationsToUsersDict = new Dictionary<int, PlayerRelation>();

        #endregion Private Fields

        #region Public Methods

        public PlayerRelation GetRelationTo(ITDSPlayer target)
        {
            if (_relationsToUsersDict.TryGetValue(target.Id, out PlayerRelation relation))
                return relation;

            return PlayerRelation.None;
        }

        public bool HasRelationTo(ITDSPlayer target, PlayerRelation relation)
            => GetRelationTo(target) == relation;

        public void SetRelation(ITDSPlayer target, PlayerRelation relation)
        {
            _relationsToUsersDict[target.Id] = relation;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadRelations()
        {
            if (_entity is null)
                return;

            _relationsToUsersDict = _entity.PlayerRelationsPlayer.ToDictionary(r => r.TargetId, r => r.Relation);
        }

        #endregion Private Methods
    }
}
