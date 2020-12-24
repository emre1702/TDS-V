using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;

namespace TDS.Server.Handler
{
    public class WorkaroundsHandler
    {
        private static readonly Dictionary<Entity, EntityAttachInfoDto> _attachedEntitiesInfos = new Dictionary<Entity, EntityAttachInfoDto>();
        private static readonly Dictionary<IBaseLobby, List<Entity>> _attachedEntitiesPerLobby = new Dictionary<IBaseLobby, List<Entity>>();

        private static readonly Dictionary<Entity, EntityCollisionlessInfoDto> _collisionslessEntitiesInfos = new Dictionary<Entity, EntityCollisionlessInfoDto>();
        private static readonly Dictionary<IBaseLobby, List<Entity>> _collisionslessEntitiesPerLobby = new Dictionary<IBaseLobby, List<Entity>>();

        private static readonly Dictionary<IBaseLobby, List<Entity>> _frozenEntityPerLobby = new Dictionary<IBaseLobby, List<Entity>>();

        private static readonly Dictionary<IBaseLobby, List<Entity>> _invincibleEntityPerLobby = new Dictionary<IBaseLobby, List<Entity>>();

        private readonly LobbiesHandler _lobbiesHandler;

        public WorkaroundsHandler(EventsHandler eventsHandler, LobbiesHandler lobbiesHandler)
        {
            _lobbiesHandler = lobbiesHandler;

            eventsHandler.PlayerJoinedLobby += PlayerJoinedLobby;
            eventsHandler.PlayerLeftLobby += PlayerLeftLobby;
        }

        public void AttachEntityToEntity(Entity entity, Entity entityTarget, PedBone bone, Vector3 positionOffset, Vector3 rotationOffset, IBaseLobby? lobby = null)
        {
            var infoDto = new EntityAttachInfoDto
            (
                EntityValue: entity.Value,
                TargetValue: entityTarget.Value,
                Bone: (int)bone,
                PositionOffsetX: positionOffset.X,
                PositionOffsetY: positionOffset.Y,
                PositionOffsetZ: positionOffset.Z,
                RotationOffsetX: rotationOffset.X,
                RotationOffsetY: rotationOffset.Y,
                RotationOffsetZ: rotationOffset.Z,
                LobbyId: lobby?.Entity.Id
            );
            infoDto.Json = Serializer.ToClient(infoDto);
            _attachedEntitiesInfos[entity] = infoDto;

            if (lobby is null)
                NAPI.Task.RunSafe(() =>
                    NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.AttachEntityToEntityWorkaround, _attachedEntitiesInfos[entity].Json));
            else
            {
                lobby.Sync.TriggerEvent(ToClientEvent.AttachEntityToEntityWorkaround, _attachedEntitiesInfos[entity].Json);

                lock (_attachedEntitiesPerLobby)
                {
                    if (!_attachedEntitiesPerLobby.ContainsKey(lobby))
                        _attachedEntitiesPerLobby[lobby] = new List<Entity>();
                    _attachedEntitiesPerLobby[lobby].Add(entity);
                }

            }
        }

        public void DetachEntity(Entity entity)
        {
            if (!_attachedEntitiesInfos.ContainsKey(entity))
                return;
            var info = _attachedEntitiesInfos[entity];
            if (info.LobbyId.HasValue)
            {
                var lobby = _lobbiesHandler.GetLobby(info.LobbyId.Value);
                if (lobby is null)
                    return;
                lobby.Sync.TriggerEvent(ToClientEvent.DetachEntityWorkaround, entity.Value);
            }
            else
                NAPI.Task.RunSafe(() =>
                    NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.DetachEntityWorkaround, entity.Value));

            _attachedEntitiesInfos.Remove(entity);
        }

        public void FreezeEntity(Entity entity, bool freeze, IBaseLobby lobby)
        {
            lobby.Sync.TriggerEvent(ToClientEvent.FreezeEntityWorkaround, entity.Handle.Value, freeze);

            if (freeze)
            {
                if (!_frozenEntityPerLobby.ContainsKey(lobby))
                    _frozenEntityPerLobby[lobby] = new List<Entity>();
                _frozenEntityPerLobby[lobby].Add(entity);
            }
            else
            {
                if (!_frozenEntityPerLobby.ContainsKey(lobby))
                    return;
                _frozenEntityPerLobby[lobby].Remove(entity);
            }
        }

        public void FreezePlayer(Player player, bool freeze)
        {
            NAPI.Task.RunSafe(() =>
                NAPI.ClientEvent.TriggerClientEvent(player, ToClientEvent.FreezePlayerWorkaround, freeze));
        }

        public void SetEntityCollisionless(Entity entity, bool collisionless, IBaseLobby? lobby = null)
        {
            var info = new EntityCollisionlessInfoDto
            (
                entityValue: entity.Value,
                collisionless: collisionless
            );
            info.Json = Serializer.ToClient(info);
            _collisionslessEntitiesInfos[entity] = info;

            if (lobby is null)
                NAPI.Task.RunSafe(() =>
                    NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.SetEntityCollisionlessWorkaround, _collisionslessEntitiesInfos[entity].Json));
            else
            {
                NAPI.Task.RunSafe(() =>
                    NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.SetEntityCollisionlessWorkaround, _collisionslessEntitiesInfos[entity].Json));

                if (!_collisionslessEntitiesPerLobby.ContainsKey(lobby))
                    _collisionslessEntitiesPerLobby[lobby] = new List<Entity>();
                _collisionslessEntitiesPerLobby[lobby].Add(entity);
            }
        }

        public void SetEntityInvincible(Player invincibleAtClient, Entity entity, bool invincible)
        {
            NAPI.Task.RunSafe(() =>
                NAPI.ClientEvent.TriggerClientEvent(invincibleAtClient, ToClientEvent.SetEntityInvincible, entity.Handle.Value, invincible));
        }

        public void SetEntityInvincible(IBaseLobby atLobby, Entity entity, bool invincible)
        {
            NAPI.Task.RunSafe(() =>
                NAPI.ClientEvent.TriggerClientEventInDimension(atLobby.MapHandler.Dimension, ToClientEvent.SetEntityInvincible, entity.Handle.Value, invincible));

            if (!_invincibleEntityPerLobby.ContainsKey(atLobby))
                _invincibleEntityPerLobby[atLobby] = new List<Entity>();
            _invincibleEntityPerLobby[atLobby].Add(entity);
        }

        public void SetPlayerInvincible(Player player, bool invincible)
        {
            NAPI.Task.RunSafe(() =>
                NAPI.ClientEvent.TriggerClientEvent(player, ToClientEvent.SetPlayerInvincible, invincible));
        }

        public void SetPlayerTeam(Player player, int team)
        {
            NAPI.Task.RunSafe(() =>
                NAPI.ClientEvent.TriggerClientEvent(player, ToClientEvent.SetPlayerTeamWorkaround, team));
        }

        private static void PlayerJoinedLobby(ITDSPlayer player, IBaseLobby lobby)
        {
            if (_attachedEntitiesPerLobby.ContainsKey(lobby))
            {
                NAPI.Task.RunSafe(() =>
                {
                    foreach (Entity entity in _attachedEntitiesPerLobby[lobby].ToArray())
                    {
                        _attachedEntitiesPerLobby[lobby].RemoveAll(e => !e.Exists);
                        if (!_attachedEntitiesInfos.ContainsKey(entity))
                        {
                            _attachedEntitiesPerLobby[lobby].Remove(entity);
                            continue;
                        }
                        player.TriggerEvent(ToClientEvent.AttachEntityToEntityWorkaround, _attachedEntitiesInfos[entity].Json);
                    }
                });

                if (_attachedEntitiesPerLobby[lobby].Count == 0)
                    _attachedEntitiesPerLobby.Remove(lobby);
            }

            if (_collisionslessEntitiesPerLobby.ContainsKey(lobby))
            {
                NAPI.Task.RunSafe(() =>
                {
                    _collisionslessEntitiesPerLobby[lobby].RemoveAll(e => !e.Exists);
                    foreach (Entity entity in _collisionslessEntitiesPerLobby[lobby])
                        player.TriggerEvent(ToClientEvent.SetEntityCollisionlessWorkaround, _collisionslessEntitiesInfos[entity].Json);
                });

                if (_collisionslessEntitiesPerLobby[lobby].Count == 0)
                    _collisionslessEntitiesPerLobby.Remove(lobby);
            }

            if (_frozenEntityPerLobby.ContainsKey(lobby))
            {
                NAPI.Task.RunSafe(() =>
                {
                    _frozenEntityPerLobby[lobby].RemoveAll(e => !e.Exists);
                    foreach (Entity entity in _frozenEntityPerLobby[lobby])
                        player.TriggerEvent(ToClientEvent.FreezeEntityWorkaround, entity.Handle.Value, true);
                });
            }

            if (_invincibleEntityPerLobby.ContainsKey(lobby))
            {

                NAPI.Task.RunSafe(() =>
                {
                    _invincibleEntityPerLobby[lobby].RemoveAll(e => !e.Exists);
                    foreach (var entity in _invincibleEntityPerLobby[lobby])
                        player.TriggerEvent(ToClientEvent.SetEntityInvincible, entity.Handle.Value, true);
                });
            }
        }

        private void PlayerLeftLobby(ITDSPlayer player, IBaseLobby lobby)
        {
        }
    }
}
