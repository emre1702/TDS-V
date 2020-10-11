using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Enums;

namespace TDS_Server.PlayersSystem
{
    public class Relations : IPlayerRelations
    {
        // Todo: Add more messages for e.g. joined gang, is now supporter etc.

        private readonly HashSet<ITDSPlayer> _friendsOnline = new HashSet<ITDSPlayer>();
        private Dictionary<int, PlayerRelation> _relationsToUsersDict = new Dictionary<int, PlayerRelation>();

        private readonly EventsHandler _eventsHandler;

#nullable disable
        private ITDSPlayer _player;
        private IPlayerEvents _events;
#nullable enable

        public Relations(EventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;
        }

        public void Init(ITDSPlayer player, IPlayerEvents events)
        {
            _player = player;
            _events = events;

            _eventsHandler.PlayerLoggedIn += CheckPlayerOnlineIsFriend;
            _eventsHandler.PlayerLoggedOut += RemovePlayerFromOnlineFriend;
            events.EntityChanged += LoadRelations;
            events.Removed += Events_Removed;
        }

        private void Events_Removed()
        {
            _eventsHandler.PlayerLoggedIn -= CheckPlayerOnlineIsFriend;
            _eventsHandler.PlayerLoggedOut -= RemovePlayerFromOnlineFriend;
            _events.EntityChanged -= LoadRelations;
            _events.Removed -= Events_Removed;
        }

        private void AddFriendPlayerEvents(ITDSPlayer friend)
        {
            friend.Events.LobbyJoined += FriendPlayerJoinedLobby;
            friend.Events.LobbyLeft += FriendPlayerLeftLobby;
        }

        private void RemoveFriendPlayerEvents(ITDSPlayer friend)
        {
            friend.Events.LobbyJoined -= FriendPlayerJoinedLobby;
            friend.Events.LobbyLeft -= FriendPlayerLeftLobby;
        }

        public PlayerRelation GetRelationTo(ITDSPlayer target)
        {
            lock (_relationsToUsersDict)
            {
                if (_relationsToUsersDict.TryGetValue(target.Id, out PlayerRelation relation))
                    return relation;
            }

            return PlayerRelation.None;
        }

        public bool HasRelationTo(ITDSPlayer target, PlayerRelation relation)
            => GetRelationTo(target) == relation;

        public void SetRelation(ITDSPlayer target, PlayerRelation relation)
        {
            PlayerRelation oldRelation;
            lock (_relationsToUsersDict)
            {
                oldRelation = _relationsToUsersDict[target.Id];
                _relationsToUsersDict[target.Id] = relation;
            }

            if (oldRelation == PlayerRelation.Friend)
                RemoveFriendPlayerEvents(target);
            if (relation == PlayerRelation.Friend)
                AddFriendPlayerEvents(target);
        }

        private void FriendPlayerJoinedLobby(ITDSPlayer otherPlayer, IBaseLobby lobby)
        {
            if (lobby.Type == LobbyType.MainMenu)
                return;

            var msg = string.Format(_player.Language.FRIEND_JOINED_LOBBY_INFO, otherPlayer.DisplayName, lobby.Entity.Name);
            NAPI.Task.Run(() => _player.Chat.SendNotification(msg));
        }

        private void FriendPlayerLeftLobby(ITDSPlayer otherPlayer, IBaseLobby lobby)
        {
            if (lobby.Type == LobbyType.MainMenu)
                return;

            var msg = string.Format(_player.Language.FRIEND_LEFT_LOBBY_INFO, otherPlayer.DisplayName, lobby.Entity.Name);
            NAPI.Task.Run(() => _player.Chat.SendNotification(msg));
        }

        private void CheckPlayerOnlineIsFriend(ITDSPlayer otherPlayer)
        {
            bool hasRelation;
            PlayerRelation relation;
            lock (_relationsToUsersDict) { hasRelation = _relationsToUsersDict.TryGetValue(otherPlayer.Id, out relation); }

            if (hasRelation && relation == PlayerRelation.Friend)
            {
                AddFriendPlayerEvents(otherPlayer);
                _friendsOnline.Add(otherPlayer);
                var msg = string.Format(_player.Language.FRIEND_LOGGEDIN_INFO, otherPlayer.DisplayName);
                NAPI.Task.Run(() => _player.Chat.SendNotification(msg));
            }
        }

        private void RemovePlayerFromOnlineFriend(ITDSPlayer otherPlayer)
        {
            if (_friendsOnline.Remove(otherPlayer))
            {
                RemoveFriendPlayerEvents(otherPlayer);
                var msg = string.Format(_player.Language.FRIEND_LOGGEDOFF_INFO, otherPlayer.DisplayName);
                NAPI.Task.Run(() => _player.Chat.SendNotification(msg));
            }
        }

        private void LoadRelations(Database.Entity.Player.Players? entity)
        {
            if (entity is null)
                return;

            _relationsToUsersDict = entity.PlayerRelationsPlayer.ToDictionary(r => r.TargetId, r => r.Relation);
        }
    }
}
