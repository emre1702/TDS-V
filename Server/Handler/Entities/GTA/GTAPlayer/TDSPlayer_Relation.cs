using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        // Todo: Add more messages for e.g. joined gang, is now supporter etc.

        private readonly HashSet<ITDSPlayer> _friendsOnline = new HashSet<ITDSPlayer>();
        private Dictionary<int, PlayerRelation> _relationsToUsersDict = new Dictionary<int, PlayerRelation>();

        public override void CheckPlayerOnlineIsFriend(ITDSPlayer otherPlayer, bool outputInfo = true)
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

        public override PlayerRelation GetRelationTo(ITDSPlayer target)
        {
            if (_relationsToUsersDict.TryGetValue(target.Id, out PlayerRelation relation))
                return relation;

            return PlayerRelation.None;
        }

        public override bool HasRelationTo(ITDSPlayer target, PlayerRelation relation)
            => GetRelationTo(target) == relation;

        public override void RemovePlayerFromOnlineFriend(ITDSPlayer otherPlayer, bool outputInfo = true)
        {
            if (_friendsOnline.Remove(otherPlayer) && outputInfo)
            {
                SendNotification(string.Format(Language.FRIEND_LOGGEDOFF_INFO, otherPlayer.DisplayName));
            }
        }

        public override void SetRelation(ITDSPlayer target, PlayerRelation relation)
        {
            _relationsToUsersDict[target.Id] = relation;
        }

        private void CheckFriendPlayerJoinedLobby(ITDSPlayer otherPlayer, IBaseLobby lobby)
        {
            if (!_friendsOnline.Contains(otherPlayer))
                return;

            if (lobby.Type == LobbyType.MainMenu)
                return;

            SendNotification(string.Format(Language.FRIEND_JOINED_LOBBY_INFO, otherPlayer.DisplayName, lobby.Entity.Name));
        }

        private void CheckFriendPlayerLeftLobby(ITDSPlayer otherPlayer, IBaseLobby lobby)
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
    }
}
