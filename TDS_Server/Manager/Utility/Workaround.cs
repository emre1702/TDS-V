using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Server.Enum;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;

namespace TDS_Server.Manager.Utility
{
    static class Workaround
    {
        private static readonly Dictionary<GTANetworkAPI.Entity, EntityAttachInfoDto> attachedEntitiesInfos = new Dictionary<GTANetworkAPI.Entity, EntityAttachInfoDto>();
        private static readonly Dictionary<Lobby, List<GTANetworkAPI.Entity>> attachedEntitiesPerLobby = new Dictionary<Lobby, List<GTANetworkAPI.Entity>>();

        private static readonly Dictionary<GTANetworkAPI.Entity, EntityCollisionlessInfoDto> collisionslessEntitiesInfos = new Dictionary<GTANetworkAPI.Entity, EntityCollisionlessInfoDto>();
        private static readonly Dictionary<Lobby, List<GTANetworkAPI.Entity>> collisionslessEntitiesPerLobby = new Dictionary<Lobby, List<GTANetworkAPI.Entity>>();

        public static void Init()
        {
            Lobby.PlayerJoinedLobby += PlayerJoinedLobby;
            Lobby.PlayerLeftLobby += PlayerLeftLobby;
        }

        public static void FreezePlayer(Client player, bool freeze)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.FreezePlayerWorkaround, freeze);
        }

        public static void SetPlayerTeam(Client player, int team)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.SetPlayerTeamWorkaround, team);
        }

        public static void UnspectatePlayer(Client player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.UnspectatePlayerWorkaround);
        }

        public static void AttachEntityToEntity(GTANetworkAPI.Entity entity, GTANetworkAPI.Entity entityTarget, EBone bone, Vector3 positionOffset, Vector3 rotationOffset, Lobby lobby = null)
        {
            var infoDto = new EntityAttachInfoDto
            {
                EntityValue = entity.Value,
                TargetValue = entityTarget.Value,
                Bone = (int)bone,
                PositionOffsetX = positionOffset.X,
                PositionOffsetY = positionOffset.Y,
                PositionOffsetZ = positionOffset.Z,
                RotationOffsetX = rotationOffset.X,
                RotationOffsetY = rotationOffset.Y,
                RotationOffsetZ = rotationOffset.Z,
                LobbyId = lobby?.Id
            };
            infoDto.Json = JsonConvert.SerializeObject(infoDto);
            attachedEntitiesInfos[entity] = infoDto;

            if (lobby == null)
                NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.AttachEntityToEntityWorkaround, attachedEntitiesInfos[entity].Json);
            else
            {
                lobby.SendAllPlayerEvent(DToClientEvent.AttachEntityToEntityWorkaround, null, attachedEntitiesInfos[entity].Json);
                if (!attachedEntitiesPerLobby.ContainsKey(lobby))
                    attachedEntitiesPerLobby[lobby] = new List<GTANetworkAPI.Entity>();
                attachedEntitiesPerLobby[lobby].Add(entity);
            }
        }

        public static void DetachEntity(GTANetworkAPI.Entity entity, bool resetCollision = true)
        {
            if (!attachedEntitiesInfos.ContainsKey(entity))
                return;
            var info = attachedEntitiesInfos[entity];
            if (info.LobbyId.HasValue)
            {
                Lobby lobby = LobbyManager.GetLobby(info.LobbyId.Value);
                if (lobby == null)
                    return;
                lobby.SendAllPlayerEvent(DToClientEvent.DetachEntityWorkaround, null, entity.Value, resetCollision);
            }
            else
                NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.DetachEntityWorkaround, entity.Value, resetCollision);

            attachedEntitiesInfos.Remove(entity);
        }

        public static void SetEntityCollisionless(GTANetworkAPI.Entity entity, bool collisionless, Lobby lobby = null)
        {
            var info = new EntityCollisionlessInfoDto
            {
                EntityValue = entity.Value,
                Collisionless = collisionless
            };
            info.Json = JsonConvert.SerializeObject(info);
            collisionslessEntitiesInfos[entity] = info;

            if (lobby == null)
                NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.SetEntityCollisionlessWorkaround, collisionslessEntitiesInfos[entity].Json);
            else
            {
                lobby.SendAllPlayerEvent(DToClientEvent.SetEntityCollisionlessWorkaround, null, collisionslessEntitiesInfos[entity].Json);
                if (!collisionslessEntitiesPerLobby.ContainsKey(lobby))
                    collisionslessEntitiesPerLobby[lobby] = new List<GTANetworkAPI.Entity>();
                collisionslessEntitiesPerLobby[lobby].Add(entity);
            }    
        }

        private static void PlayerJoinedLobby(Lobby lobby, TDSPlayer player)
        {
            if (attachedEntitiesPerLobby.ContainsKey(lobby))
            {
                attachedEntitiesPerLobby[lobby].RemoveAll(e => !e.Exists);
                foreach (GTANetworkAPI.Entity entity in attachedEntitiesPerLobby[lobby])
                {
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.AttachEntityToEntityWorkaround, attachedEntitiesInfos[entity].Json);
                }
                if (attachedEntitiesPerLobby[lobby].Count == 0)
                    attachedEntitiesPerLobby.Remove(lobby);
            }

            if (collisionslessEntitiesPerLobby.ContainsKey(lobby))
            {
                collisionslessEntitiesPerLobby[lobby].RemoveAll(e => !e.Exists);
                foreach (GTANetworkAPI.Entity entity in collisionslessEntitiesPerLobby[lobby])
                {
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SetEntityCollisionlessWorkaround, collisionslessEntitiesInfos[entity].Json);
                }
                if (collisionslessEntitiesPerLobby[lobby].Count == 0)
                    collisionslessEntitiesPerLobby.Remove(lobby);
            }
        }

        private static void PlayerLeftLobby(Lobby lobby, TDSPlayer player)
        {

        }
    }
}
