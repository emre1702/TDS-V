using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;
using TDS_Shared.Core;

namespace TDS_Server.RAGEAPI
{
    internal class WorkaroundsHandler
    {
        private static readonly Dictionary<GTANetworkAPI.Entity, EntityAttachInfoDto> _attachedEntitiesInfos = new Dictionary<GTANetworkAPI.Entity, EntityAttachInfoDto>();
        private static readonly Dictionary<ILobby, List<GTANetworkAPI.Entity>> _attachedEntitiesPerLobby = new Dictionary<ILobby, List<GTANetworkAPI.Entity>>();

        private static readonly Dictionary<GTANetworkAPI.Entity, EntityCollisionlessInfoDto> _collisionslessEntitiesInfos = new Dictionary<GTANetworkAPI.Entity, EntityCollisionlessInfoDto>();
        private static readonly Dictionary<ILobby, List<GTANetworkAPI.Entity>> _collisionslessEntitiesPerLobby = new Dictionary<ILobby, List<GTANetworkAPI.Entity>>();

        private static readonly Dictionary<ILobby, List<GTANetworkAPI.Entity>> _frozenEntityPerLobby = new Dictionary<ILobby, List<GTANetworkAPI.Entity>>();

        private static readonly Dictionary<ILobby, List<GTANetworkAPI.Entity>> _invincibleEntityPerLobby = new Dictionary<ILobby, List<GTANetworkAPI.Entity>>();

        private readonly IModAPI _modAPI;
        private readonly Serializer _serializer;
        private readonly LobbiesHandler _lobbiesHandler;

        internal WorkaroundsHandler(EventsHandler eventsHandler, IModAPI modAPI, LobbiesHandler lobbiesHandler)
        {
            _modAPI = modAPI;
            _serializer = new Serializer();
            _lobbiesHandler = lobbiesHandler;

            eventsHandler.PlayerJoinedLobby += PlayerJoinedLobby;
            eventsHandler.PlayerLeftLobby += PlayerLeftLobby;
        }

        public void FreezePlayer(GTANetworkAPI.Player player, bool freeze)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, ToClientEvent.FreezePlayerWorkaround, freeze);
        }

        public void FreezeEntity(GTANetworkAPI.Entity entity, bool freeze, ILobby lobby)
        {
            _modAPI.Sync.SendEvent(lobby, ToClientEvent.FreezeEntityWorkaround, entity.Handle.Value, freeze);

            if (freeze)
            {
                if (!_frozenEntityPerLobby.ContainsKey(lobby))
                    _frozenEntityPerLobby[lobby] = new List<GTANetworkAPI.Entity>();
                _frozenEntityPerLobby[lobby].Add(entity);
            }
            else
            {
                if (!_frozenEntityPerLobby.ContainsKey(lobby))
                    return;
                _frozenEntityPerLobby[lobby].Remove(entity);
            }
        }

        public void SetPlayerTeam(GTANetworkAPI.Player player, int team)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, ToClientEvent.SetPlayerTeamWorkaround, team);
        }

        public void AttachEntityToEntity(GTANetworkAPI.Entity entity, GTANetworkAPI.Entity entityTarget, PedBone bone, Vector3 positionOffset, Vector3 rotationOffset, ILobby? lobby = null)
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
                LobbyId: lobby?.Id
            );
            infoDto.Json = _serializer.ToClient(infoDto);
            _attachedEntitiesInfos[entity] = infoDto;

            if (lobby is null)
                NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.AttachEntityToEntityWorkaround, _attachedEntitiesInfos[entity].Json);
            else
            {
                _modAPI.Sync.SendEvent(lobby, ToClientEvent.AttachEntityToEntityWorkaround, _attachedEntitiesInfos[entity].Json);

                if (!_attachedEntitiesPerLobby.ContainsKey(lobby))
                    _attachedEntitiesPerLobby[lobby] = new List<GTANetworkAPI.Entity>();
                _attachedEntitiesPerLobby[lobby].Add(entity);
            }
        }

        public void DetachEntity(GTANetworkAPI.Entity entity)
        {
            if (!_attachedEntitiesInfos.ContainsKey(entity))
                return;
            var info = _attachedEntitiesInfos[entity];
            if (info.LobbyId.HasValue)
            {
                ILobby? lobby = _lobbiesHandler.GetLobby(info.LobbyId.Value);
                if (lobby is null)
                    return;
                _modAPI.Sync.SendEvent(ToClientEvent.DetachEntityWorkaround, entity.Value);
            }
            else
                NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.DetachEntityWorkaround, entity.Value);

            _attachedEntitiesInfos.Remove(entity);
        }

        public void SetEntityCollisionless(GTANetworkAPI.Entity entity, bool collisionless, ILobby? lobby = null)
        {
            var info = new EntityCollisionlessInfoDto
            (
                entityValue: entity.Value,
                collisionless: collisionless
            );
            info.Json = _serializer.ToClient(info);
            _collisionslessEntitiesInfos[entity] = info;

            if (lobby is null)
                NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.SetEntityCollisionlessWorkaround, _collisionslessEntitiesInfos[entity].Json);
            else
            {
                _modAPI.Sync.SendEvent(ToClientEvent.SetEntityCollisionlessWorkaround, _collisionslessEntitiesInfos[entity].Json);

                if (!_collisionslessEntitiesPerLobby.ContainsKey(lobby))
                    _collisionslessEntitiesPerLobby[lobby] = new List<GTANetworkAPI.Entity>();
                _collisionslessEntitiesPerLobby[lobby].Add(entity);
            }
        }

        public void SetPlayerInvincible(GTANetworkAPI.Player player, bool invincible)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, ToClientEvent.SetPlayerInvincible, invincible);
        }

        public void SetEntityInvincible(GTANetworkAPI.Player invincibleAtClient, GTANetworkAPI.Entity entity, bool invincible)
        {
            NAPI.ClientEvent.TriggerClientEvent(invincibleAtClient, ToClientEvent.SetEntityInvincible, entity.Handle.Value, invincible);
        }

        public void SetEntityInvincible(ILobby atLobby, GTANetworkAPI.Entity entity, bool invincible)
        {
            NAPI.ClientEvent.TriggerClientEventInDimension(atLobby.Dimension, ToClientEvent.SetEntityInvincible, entity.Handle.Value, invincible);

            if (!_invincibleEntityPerLobby.ContainsKey(atLobby))
                _invincibleEntityPerLobby[atLobby] = new List<GTANetworkAPI.Entity>();
            _invincibleEntityPerLobby[atLobby].Add(entity);
        }


        private static void PlayerJoinedLobby(ITDSPlayer player, ILobby lobby)
        {
            if (_attachedEntitiesPerLobby.ContainsKey(lobby))
            {
                _attachedEntitiesPerLobby[lobby].RemoveAll(e => !e.Exists);
                foreach (GTANetworkAPI.Entity entity in _attachedEntitiesPerLobby[lobby].ToArray())
                {
                    if (!_attachedEntitiesInfos.ContainsKey(entity))
                    {
                        _attachedEntitiesPerLobby[lobby].Remove(entity);
                        continue;
                    }
                    player.SendEvent(ToClientEvent.AttachEntityToEntityWorkaround, _attachedEntitiesInfos[entity].Json);
                }
                if (_attachedEntitiesPerLobby[lobby].Count == 0)
                    _attachedEntitiesPerLobby.Remove(lobby);
            }

            if (_collisionslessEntitiesPerLobby.ContainsKey(lobby))
            {
                _collisionslessEntitiesPerLobby[lobby].RemoveAll(e => !e.Exists);
                foreach (GTANetworkAPI.Entity entity in _collisionslessEntitiesPerLobby[lobby])
                {
                    player.SendEvent(ToClientEvent.SetEntityCollisionlessWorkaround, _collisionslessEntitiesInfos[entity].Json);
                }
                if (_collisionslessEntitiesPerLobby[lobby].Count == 0)
                    _collisionslessEntitiesPerLobby.Remove(lobby);
            }

            if (_frozenEntityPerLobby.ContainsKey(lobby))
            {
                _frozenEntityPerLobby[lobby].RemoveAll(e => !e.Exists);
                foreach (GTANetworkAPI.Entity entity in _frozenEntityPerLobby[lobby])
                {
                    player.SendEvent(ToClientEvent.FreezeEntityWorkaround, entity.Handle.Value, true);
                }
            }

            if (_invincibleEntityPerLobby.ContainsKey(lobby))
            {
                _invincibleEntityPerLobby[lobby].RemoveAll(e => !e.Exists);
                foreach (var entity in _invincibleEntityPerLobby[lobby])
                {
                    player.SendEvent(ToClientEvent.SetEntityInvincible, entity.Handle.Value, true);
                }
            }
        }

        private static void PlayerLeftLobby(ITDSPlayer player, ILobby lobby)
        {
        }
    }
}
