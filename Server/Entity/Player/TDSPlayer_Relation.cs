using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        // Todo: Add more messages for e.g. joined gang, is now supporter etc.

        #region Private Fields

        private readonly HashSet<ITDSPlayer> _friendsOnline = new HashSet<ITDSPlayer>();
        private Dictionary<int, PlayerRelation> _relationsToUsersDict = new Dictionary<int, PlayerRelation>();

        #endregion Private Fields

        #region Public Methods

        public void CheckPlayerOnlineIsFriend(ITDSPlayer otherPlayer, bool outputInfo = true)
        {
            if (_relationsToUsersDict.TryGetValue(otherPlayer.Id, out PlayerRelation relation) && relation == PlayerRelation.Friend)
            {
                _friendsOnline.Add(otherPlayer);
                if (outputInfo)
                {
                    SendNotification(string.Format(Language.FRIEND_LOGGEDIN_INFO, otherPlayer.DisplayName));
                }
            }
        }

        public PlayerRelation GetRelationTo(ITDSPlayer target)
        {
            if (_relationsToUsersDict.TryGetValue(target.Id, out PlayerRelation relation))
                return relation;

            return PlayerRelation.None;
        }

        public bool HasRelationTo(ITDSPlayer target, PlayerRelation relation)
            => GetRelationTo(target) == relation;

        public void RemovePlayerFromOnlineFriend(ITDSPlayer otherPlayer, bool outputInfo = true)
        {
            if (_friendsOnline.Remove(otherPlayer) && outputInfo)
            {
                SendNotification(string.Format(Language.FRIEND_LOGGEDOFF_INFO, otherPlayer.DisplayName));
            }
        }

        public void SetRelation(ITDSPlayer target, PlayerRelation relation)
        {
            _relationsToUsersDict[target.Id] = relation;
        }

        #endregion Public Methods

        #region Private Methods

        private void CheckFriendPlayerJoinedLobby(ITDSPlayer otherPlayer, ILobby lobby)
        {
            if (!_friendsOnline.Contains(otherPlayer))
                return;

            if (lobby.Type == LobbyType.MainMenu)
                return;

            SendNotification(string.Format(Language.FRIEND_JOINED_LOBBY_INFO, otherPlayer.DisplayName, lobby.Entity.Name));
        }

        private void CheckFriendPlayerLeftLobby(ITDSPlayer otherPlayer, ILobby lobby)
        {
            if (!_friendsOnline.Contains(otherPlayer))
                return;

            if (lobby.Type == LobbyType.MainMenu)
                return;

            SendNotification(string.Format(Language.FRIEND_LEFT_LOBBY_INFO, otherPlayer.DisplayName, lobby.Entity.Name));
        }

        private void LoadRelations()
        {
            if (_entity is null)
                return;

            _relationsToUsersDict = _entity.PlayerRelationsPlayer.ToDictionary(r => r.TargetId, r => r.Relation);
        }

        #endregion Private Methods
    }
}
