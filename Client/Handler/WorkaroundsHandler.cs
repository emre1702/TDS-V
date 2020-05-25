using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class WorkaroundsHandler : ServiceBase
    {
        #region Private Fields

        private readonly Serializer _serializer;
        private readonly UtilsHandler _utilsHandler;

        #endregion Private Fields

        #region Public Constructors

        public WorkaroundsHandler(IModAPI modAPI, LoggingHandler loggingHandler, Serializer serializer, UtilsHandler utilsHandler)
            : base(modAPI, loggingHandler)
        {
            _serializer = serializer;
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

        #endregion Public Constructors

        #region Public Methods

        public void AttachEntityToEntityWorkaroundMethod(object[] args)
        {
            EntityAttachInfoDto info = _serializer.FromServer<EntityAttachInfoDto>(args[0].ToString());
            info.EntityValue = ModAPI.Pool.Objects.GetAtRemote((ushort)info.EntityValue).Handle;
            info.TargetValue = ModAPI.Pool.Players.GetAtRemote((ushort)info.TargetValue).Handle;
            ModAPI.Entity.AttachEntityToEntity(info.EntityValue, info.TargetValue, ModAPI.Ped.GetPedBoneIndex(info.TargetValue, info.Bone),
                info.PositionOffsetX, info.PositionOffsetY, info.PositionOffsetZ,
                info.RotationOffsetX, info.RotationOffsetY, info.RotationOffsetZ,
                true, true, false, false, 0, true);
        }

        public void DetachEntityWorkaroundMethod(object[] args)
        {
            int entity = (int)args[0];
            entity = ModAPI.Pool.Objects.GetAtRemote((ushort)entity).Handle;
            ModAPI.Entity.DetachEntity(entity);
        }

        public void FreezeEntityWorkaroundMethod(object[] args)
        {
            var objHandleValue = Convert.ToUInt16(args[0]);
            bool freeze = Convert.ToBoolean(args[1]);

            var obj = ModAPI.Pool.Objects.GetAtRemote(objHandleValue);
            if (obj is null)
                return;
            obj.FreezePosition(freeze);
        }

        public void FreezePlayerWorkaroundMethod(object[] args)
        {
            bool freeze = Convert.ToBoolean(args[0]);
            ModAPI.LocalPlayer.FreezePosition(freeze);
        }

        public void SetEntityCollisionlessWorkaroundMethod(object[] args)
        {
            EntityCollisionlessInfoDto info = _serializer.FromServer<EntityCollisionlessInfoDto>(args[0].ToString());
            IEntityBase entity = ModAPI.Pool.Objects.GetAtRemote((ushort)info.EntityValue);
            if (entity == null)
            {
                entity = _utilsHandler.GetPlayerByHandleValue((ushort)info.EntityValue);
            }
            if (entity == null)
                return;

            info.EntityValue = entity.Handle;
            ModAPI.Entity.SetEntityCollision(info.EntityValue, !info.Collisionless, true);
        }

        public void SetEntityInvincibleMethod(object[] args)
        {
            ushort handle = Convert.ToUInt16(args[0]);
            bool toggle = Convert.ToBoolean(args[1]);

            var vehHandle = ModAPI.Pool.Vehicles.GetAtRemote(handle)?.Handle;
            if (vehHandle.HasValue)
                ModAPI.Entity.SetEntityInvincible(vehHandle.Value, toggle);

            var objHandle = ModAPI.Pool.Objects.GetAtRemote(handle)?.Handle;
            if (objHandle.HasValue)
                ModAPI.Entity.SetEntityInvincible(objHandle.Value, toggle);
        }

        public void SetPlayerInvincibleMethod(object[] args)
        {
            bool toggle = Convert.ToBoolean(args[0]);
            ModAPI.LocalPlayer.SetInvincible(toggle);
        }

        public void SetPlayerTeamWorkaroundMethod(object[] args)
        {
            int team = (int)args[0];
            ModAPI.Player.SetPlayerTeam(team);
        }

        #endregion Public Methods
    }
}
