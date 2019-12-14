using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Server.Enums;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.EventManager;

namespace TDS_Server.Manager.Utility
{
    internal static class Workaround
    {
        private static readonly Dictionary<Entity, EntityAttachInfoDto> _attachedEntitiesInfos = new Dictionary<Entity, EntityAttachInfoDto>();
        private static readonly Dictionary<Lobby, List<Entity>> _attachedEntitiesPerLobby = new Dictionary<Lobby, List<Entity>>();

        private static readonly Dictionary<Entity, EntityCollisionlessInfoDto> _collisionslessEntitiesInfos = new Dictionary<Entity, EntityCollisionlessInfoDto>();
        private static readonly Dictionary<Lobby, List<Entity>> _collisionslessEntitiesPerLobby = new Dictionary<Lobby, List<Entity>>();

        public static void Init()
        {
            CustomEventManager.OnPlayerJoinedLobby += PlayerJoinedLobby;
            CustomEventManager.OnPlayerLeftLobby += PlayerLeftLobby;
        }

        public static void FreezePlayer(Client player, bool freeze)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.FreezePlayerWorkaround, freeze);
        }

        public static void SetPlayerTeam(Client player, int team)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.SetPlayerTeamWorkaround, team);
        }

        public static void AttachEntityToEntity(Entity entity, Entity entityTarget, EBone bone, Vector3 positionOffset, Vector3 rotationOffset, Lobby? lobby = null)
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
            _attachedEntitiesInfos[entity] = infoDto;

            if (lobby is null)
                NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.AttachEntityToEntityWorkaround, _attachedEntitiesInfos[entity].Json);
            else
            {
                lobby.SendAllPlayerEvent(DToClientEvent.AttachEntityToEntityWorkaround, null, _attachedEntitiesInfos[entity].Json);
                if (!_attachedEntitiesPerLobby.ContainsKey(lobby))
                    _attachedEntitiesPerLobby[lobby] = new List<Entity>();
                _attachedEntitiesPerLobby[lobby].Add(entity);
            }
        }

        public static void DetachEntity(Entity entity, bool resetCollision = true)
        {
            if (!_attachedEntitiesInfos.ContainsKey(entity))
                return;
            var info = _attachedEntitiesInfos[entity];
            if (info.LobbyId.HasValue)
            {
                Lobby? lobby = LobbyManager.GetLobby(info.LobbyId.Value);
                if (lobby is null)
                    return;
                lobby.SendAllPlayerEvent(DToClientEvent.DetachEntityWorkaround, null, entity.Value, resetCollision);
            }
            else
                NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.DetachEntityWorkaround, entity.Value, resetCollision);

            _attachedEntitiesInfos.Remove(entity);
        }

        public static void SetEntityCollisionless(Entity entity, bool collisionless, Lobby? lobby = null)
        {
            var info = new EntityCollisionlessInfoDto
            (
                EntityValue: entity.Value,
                Collisionless: collisionless
            );
            _collisionslessEntitiesInfos[entity] = info;

            if (lobby is null)
                NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.SetEntityCollisionlessWorkaround, _collisionslessEntitiesInfos[entity].Json);
            else
            {
                lobby.SendAllPlayerEvent(DToClientEvent.SetEntityCollisionlessWorkaround, null, _collisionslessEntitiesInfos[entity].Json);
                if (!_collisionslessEntitiesPerLobby.ContainsKey(lobby))
                    _collisionslessEntitiesPerLobby[lobby] = new List<Entity>();
                _collisionslessEntitiesPerLobby[lobby].Add(entity);
            }
        }

        public static void SetPlayerInvincible(Client player, bool invincible)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.SetPlayerInvincible, invincible);
        }

        public static void SetEntityInvincible(Client invincibleAtClient, Entity entity, bool invincible)
        {
            NAPI.ClientEvent.TriggerClientEvent(invincibleAtClient, DToClientEvent.SetEntityInvincible, entity.Handle.Value, invincible);
        }


        private static void PlayerJoinedLobby(TDSPlayer player, Lobby lobby)
        {
            if (_attachedEntitiesPerLobby.ContainsKey(lobby))
            {
                _attachedEntitiesPerLobby[lobby].RemoveAll(e => !e.Exists);
                foreach (Entity entity in _attachedEntitiesPerLobby[lobby].ToArray())
                {
                    if (!_attachedEntitiesInfos.ContainsKey(entity))
                    {
                        _attachedEntitiesPerLobby[lobby].Remove(entity);
                        continue;
                    }
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.AttachEntityToEntityWorkaround, _attachedEntitiesInfos[entity].Json);
                }
                if (_attachedEntitiesPerLobby[lobby].Count == 0)
                    _attachedEntitiesPerLobby.Remove(lobby);
            }

            if (_collisionslessEntitiesPerLobby.ContainsKey(lobby))
            {
                _collisionslessEntitiesPerLobby[lobby].RemoveAll(e => !e.Exists);
                foreach (Entity entity in _collisionslessEntitiesPerLobby[lobby])
                {
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SetEntityCollisionlessWorkaround, _collisionslessEntitiesInfos[entity].Json);
                }
                if (_collisionslessEntitiesPerLobby[lobby].Count == 0)
                    _collisionslessEntitiesPerLobby.Remove(lobby);
            }
        }

        private static void PlayerLeftLobby(TDSPlayer player, Lobby lobby)
        {
        }
    }
}