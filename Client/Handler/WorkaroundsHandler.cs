using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class WorkaroundsHandler
    {
        private readonly Serializer _serializer;
        private readonly IModAPI _modAPI;
        private readonly UtilsHandler _utilsHandler;

        public WorkaroundsHandler(Serializer serializer, IModAPI modAPI, UtilsHandler utilsHandler)
        {
            _serializer = serializer;
            _modAPI = modAPI;
            _utilsHandler = utilsHandler;

            modAPI.Event.Add(ToClientEvent.AttachEntityToEntityWorkaround, AttachEntityToEntityWorkaroundMethod);
            modAPI.Event.Add(ToClientEvent.DetachEntityWorkaround, DetachEntityWorkaroundMethod);
            modAPI.Event.Add(ToClientEvent.FreezeEntityWorkaround, FreezeEntityWorkaroundMethod);
            modAPI.Event.Add(ToClientEvent.FreezePlayerWorkaround, FreezePlayerWorkaroundMethod);
            modAPI.Event.Add(ToClientEvent.SetEntityCollisionlessWorkaround, SetEntityCollisionlessWorkaroundMethod);
            modAPI.Event.Add(ToClientEvent.SetEntityInvincible, SetEntityInvincibleMethod);
            modAPI.Event.Add(ToClientEvent.SetPlayerInvincible, SetPlayerInvincibleMethod);
            modAPI.Event.Add(ToClientEvent.SetPlayerTeamWorkaround, SetPlayerTeamWorkaroundMethod);
        }

        public void AttachEntityToEntityWorkaroundMethod(object[] args)
        {
            EntityAttachInfoDto info = _serializer.FromServer<EntityAttachInfoDto>(args[0].ToString());
            info.EntityValue = _modAPI.Pool.Objects.GetAtRemote((ushort)info.EntityValue).Handle;
            info.TargetValue = _modAPI.Pool.Players.GetAtRemote((ushort)info.TargetValue).Handle;
            _modAPI.Entity.AttachEntityToEntity(info.EntityValue, info.TargetValue, _modAPI.Ped.GetPedBoneIndex(info.TargetValue, info.Bone),
                info.PositionOffsetX, info.PositionOffsetY, info.PositionOffsetZ,
                info.RotationOffsetX, info.RotationOffsetY, info.RotationOffsetZ,
                true, true, false, false, 0, true);
        }

        public void DetachEntityWorkaroundMethod(object[] args)
        {
            int entity = (int)args[0];
            entity = _modAPI.Pool.Objects.GetAtRemote((ushort)entity).Handle;
            _modAPI.Entity.DetachEntity(entity);
        }

        public void FreezeEntityWorkaroundMethod(object[] args)
        {
            var objHandleValue = Convert.ToUInt16(args[0]);
            bool freeze = Convert.ToBoolean(args[1]);

            var obj = _modAPI.Pool.Objects.GetAtRemote(objHandleValue);
            if (obj is null)
                return;
            obj.FreezePosition(freeze);
        }

        public void FreezePlayerWorkaroundMethod(object[] args)
        {
            bool freeze = Convert.ToBoolean(args[0]);
            _modAPI.LocalPlayer.FreezePosition(freeze);
        }

        public void SetEntityCollisionlessWorkaroundMethod(object[] args)
        {
            EntityCollisionlessInfoDto info = _serializer.FromServer<EntityCollisionlessInfoDto>(args[0].ToString());
            IEntityBase entity = _modAPI.Pool.Objects.GetAtRemote((ushort)info.EntityValue);
            if (entity == null)
            {
                entity = _utilsHandler.GetPlayerByHandleValue((ushort)info.EntityValue);
            }
            if (entity == null)
                return;

            info.EntityValue = entity.Handle;
            _modAPI.Entity.SetEntityCollision(info.EntityValue, !info.Collisionless, true);
        }

        public void SetPlayerTeamWorkaroundMethod(object[] args)
        {
            int team = (int)args[0];
            _modAPI.Player.SetPlayerTeam(team);
        }

        public void SetEntityInvincibleMethod(object[] args)
        {
            ushort handle = Convert.ToUInt16(args[0]);
            bool toggle = Convert.ToBoolean(args[1]);

            var vehHandle = _modAPI.Pool.Vehicles.GetAtRemote(handle)?.Handle;
            if (vehHandle.HasValue)
                _modAPI.Entity.SetEntityInvincible(vehHandle.Value, toggle);

            var objHandle = _modAPI.Pool.Objects.GetAtRemote(handle)?.Handle;
            if (objHandle.HasValue)
                _modAPI.Entity.SetEntityInvincible(objHandle.Value, toggle);
        }

        public void SetPlayerInvincibleMethod(object[] args)
        {
            bool toggle = Convert.ToBoolean(args[0]);
            _modAPI.LocalPlayer.SetInvincible(toggle);
        }
    }
}
