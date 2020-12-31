using RAGE.Elements;
using System;
using TDS.Shared.Core;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;

namespace TDS.Client.Handler
{
    public class WorkaroundsHandler : ServiceBase
    {
        private readonly UtilsHandler _utilsHandler;

        public WorkaroundsHandler(LoggingHandler loggingHandler, UtilsHandler utilsHandler)
            : base(loggingHandler)
        {
            _utilsHandler = utilsHandler;

            RAGE.Events.Add(ToClientEvent.AttachEntityToEntityWorkaround, AttachEntityToEntityWorkaroundMethod);
            RAGE.Events.Add(ToClientEvent.DetachEntityWorkaround, DetachEntityWorkaroundMethod);
            RAGE.Events.Add(ToClientEvent.FreezeEntityWorkaround, FreezeEntityWorkaroundMethod);
            RAGE.Events.Add(ToClientEvent.FreezePlayerWorkaround, FreezePlayerWorkaroundMethod);
            RAGE.Events.Add(ToClientEvent.SetEntityCollisionlessWorkaround, SetEntityCollisionlessWorkaroundMethod);
            RAGE.Events.Add(ToClientEvent.SetEntityInvincible, SetEntityInvincibleMethod);
            RAGE.Events.Add(ToClientEvent.SetPlayerInvincible, SetPlayerInvincibleMethod);
            RAGE.Events.Add(ToClientEvent.SetPlayerTeamWorkaround, SetPlayerTeamWorkaroundMethod);
        }

        public void AttachEntityToEntityWorkaroundMethod(object[] args)
        {
            EntityAttachInfoDto info = Serializer.FromServer<EntityAttachInfoDto>(args[0].ToString());
            var entity = RAGE.Elements.Entities.Objects.GetAtRemote((ushort)info.EntityValue);
            var target = RAGE.Elements.Entities.Players.GetAtRemote((ushort)info.TargetValue);
            if (entity is null || target is null)
                return;     // Not sure if this is good.
            info.EntityValue = entity.Handle;
            info.TargetValue = target.Handle;
            RAGE.Game.Entity.AttachEntityToEntity(info.EntityValue, info.TargetValue, RAGE.Game.Ped.GetPedBoneIndex(info.TargetValue, info.Bone),
                info.PositionOffsetX, info.PositionOffsetY, info.PositionOffsetZ,
                info.RotationOffsetX, info.RotationOffsetY, info.RotationOffsetZ,
                true, true, false, false, 0, true);
        }

        public void DetachEntityWorkaroundMethod(object[] args)
        {
            int entity = (int)args[0];
            entity = RAGE.Elements.Entities.Objects.GetAtRemote((ushort)entity).Handle;
            RAGE.Game.Entity.DetachEntity(entity, true, true);
        }

        public void FreezeEntityWorkaroundMethod(object[] args)
        {
            var objHandleValue = Convert.ToUInt16(args[0]);
            bool freeze = Convert.ToBoolean(args[1]);

            var obj = RAGE.Elements.Entities.Objects.GetAtRemote(objHandleValue);
            if (obj is null)
                return;
            obj.FreezePosition(freeze);
        }

        public void FreezePlayerWorkaroundMethod(object[] args)
        {
            bool freeze = Convert.ToBoolean(args[0]);
            Player.LocalPlayer.FreezePosition(freeze);
        }

        public void SetEntityCollisionlessWorkaroundMethod(object[] args)
        {
            EntityCollisionlessInfoDto info = Serializer.FromServer<EntityCollisionlessInfoDto>(args[0].ToString());
            GameEntityBase entity = RAGE.Elements.Entities.Objects.GetAtRemote((ushort)info.EntityValue);
            if (entity == null)
            {
                entity = _utilsHandler.GetPlayerByHandleValue((ushort)info.EntityValue);
            }
            if (entity == null)
                return;

            info.EntityValue = entity.Handle;
            RAGE.Game.Entity.SetEntityCollision(info.EntityValue, !info.Collisionless, true);
        }

        public void SetEntityInvincibleMethod(object[] args)
        {
            ushort handle = Convert.ToUInt16(args[0]);
            bool toggle = Convert.ToBoolean(args[1]);

            var vehHandle = RAGE.Elements.Entities.Vehicles.GetAtRemote(handle)?.Handle;
            if (vehHandle.HasValue)
                RAGE.Game.Entity.SetEntityInvincible(vehHandle.Value, toggle);

            var objHandle = RAGE.Elements.Entities.Objects.GetAtRemote(handle)?.Handle;
            if (objHandle.HasValue)
                RAGE.Game.Entity.SetEntityInvincible(objHandle.Value, toggle);
        }

        public void SetPlayerInvincibleMethod(object[] args)
        {
            bool toggle = Convert.ToBoolean(args[0]);
            RAGE.Elements.Player.LocalPlayer.SetInvincible(toggle);
        }

        public void SetPlayerTeamWorkaroundMethod(object[] args)
        {
            int team = (int)args[0];
            RAGE.Game.Player.SetPlayerTeam(team);
        }
    }
}