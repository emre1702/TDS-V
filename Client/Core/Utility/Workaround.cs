using RAGE.Elements;
using System;
using System.Linq;
using TDS_Shared.Data.Models;
using TDS_Shared.Core;

namespace TDS_Client.Manager.Utility
{
    internal static class Workaround
    {
        public static void AttachEntityToEntityWorkaroundMethod(object[] args)
        {
            EntityAttachInfoDto info = Serializer.FromServer<EntityAttachInfoDto>(args[0].ToString());
            info.EntityValue = Entities.Objects.GetAtRemote((ushort)info.EntityValue).Handle;
            info.TargetValue = Entities.Players.GetAtRemote((ushort)info.TargetValue).Handle;
            RAGE.Game.Entity.AttachEntityToEntity(info.EntityValue, info.TargetValue, RAGE.Game.Ped.GetPedBoneIndex(info.TargetValue, info.Bone),
                info.PositionOffsetX, info.PositionOffsetY, info.PositionOffsetZ,
                info.RotationOffsetX, info.RotationOffsetY, info.RotationOffsetZ,
                true, true, false, false, 0, true);
        }

        public static void DetachEntityWorkaroundMethod(object[] args)
        {
            int entity = (int)args[0];
            entity = Entities.Objects.GetAtRemote((ushort)entity).Handle;
            bool resetCollision = Convert.ToBoolean(args[1]);
            RAGE.Game.Entity.DetachEntity(entity, true, resetCollision);
        }

        public static void FreezeEntityWorkaroundMethod(object[] args)
        {
            var objHandleValue = Convert.ToUInt16(args[0]);
            bool freeze = Convert.ToBoolean(args[1]);

            var obj = Entities.Objects.All.FirstOrDefault(o => o.RemoteId == objHandleValue);
            if (obj is null)
                return;
            obj.FreezePosition(freeze);
        }

        public static void FreezePlayerWorkaroundMethod(object[] args)
        {
            bool freeze = Convert.ToBoolean(args[0]);
            Player.LocalPlayer.FreezePosition(freeze);
        }

        public static void SetEntityCollisionlessWorkaroundMethod(object[] args)
        {
            EntityCollisionlessInfoDto info = Serializer.FromServer<EntityCollisionlessInfoDto>(args[0].ToString());
            GameEntity entity = Entities.Objects.GetAtRemote((ushort)info.EntityValue);
            if (entity == null)
            {
                entity = ClientUtils.GetPlayerByHandleValue((ushort)info.EntityValue);
            }
            if (entity == null)
                return;

            info.EntityValue = entity.Handle;
            RAGE.Game.Entity.SetEntityCollision(info.EntityValue, !info.Collisionless, true);
        }

        public static void SetPlayerTeamWorkaroundMethod(object[] args)
        {
            int team = (int)args[0];
            RAGE.Game.Player.SetPlayerTeam(team);
        }

        public static void SetEntityInvincibleMethod(object[] args)
        {
            ushort handle = Convert.ToUInt16(args[0]);
            bool toggle = Convert.ToBoolean(args[1]);

            var vehHandle = Entities.Vehicles.GetAtRemote(handle)?.Handle;
            if (vehHandle.HasValue)
                RAGE.Game.Entity.SetEntityInvincible(vehHandle.Value, toggle);

            var objHandle = Entities.Objects.GetAtRemote(handle)?.Handle;
            if (objHandle.HasValue)
                RAGE.Game.Entity.SetEntityInvincible(objHandle.Value, toggle);
        }

        public static void SetPlayerInvincibleMethod(object[] args)
        {
            bool toggle = Convert.ToBoolean(args[0]);
            Player.LocalPlayer.SetInvincible(toggle);
        }
    }
}
