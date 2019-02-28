using Newtonsoft.Json;
using RAGE;
using RAGE.Elements;
using TDS_Common.Default;
using TDS_Common.Dto;

namespace TDS_Client.Manager.Utility
{
    static class Workaround
    {
        public static void AddEvents()
        {
            Events.Add(DToClientEvent.AttachEntityToEntityWorkaround, AttachEntityToEntityWorkaroundMethod);
            Events.Add(DToClientEvent.DetachEntityWorkaround, DetachEntityWorkaroundMethod);
            Events.Add(DToClientEvent.FreezePlayerWorkaround, FreezePlayerWorkaroundMethod);
            Events.Add(DToClientEvent.SetEntityCollisionlessWorkaround, SetEntityCollisionlessWorkaroundMethod);
            Events.Add(DToClientEvent.SetPlayerTeamWorkaround, SetPlayerTeamWorkaroundMethod);
            Events.Add(DToClientEvent.UnspectatePlayerWorkaround, UnspectatePlayerWorkaroundMethod);
        }

        private static void AttachEntityToEntityWorkaroundMethod(object[] args)
        {
            EntityAttachInfoDto info = JsonConvert.DeserializeObject<EntityAttachInfoDto>(args[0].ToString());
            info.EntityValue = Entities.Objects.GetAtRemote((ushort)info.EntityValue).Handle;
            info.TargetValue = Entities.Players.GetAtRemote((ushort)info.TargetValue).Handle;
            RAGE.Game.Entity.AttachEntityToEntity(info.EntityValue, info.TargetValue, RAGE.Game.Ped.GetPedBoneIndex(info.TargetValue, info.Bone), 
                info.PositionOffsetX, info.PositionOffsetY, info.PositionOffsetZ, 
                info.RotationOffsetX, info.RotationOffsetY, info.RotationOffsetZ,
                true, true, false, false, 0, true);
        }

        private static void DetachEntityWorkaroundMethod(object[] args)
        {
            int entity = (int)args[0];
            entity = Entities.Objects.GetAtRemote((ushort)entity).Handle;
            bool resetCollision = (bool)args[1];
            RAGE.Game.Entity.DetachEntity(entity, true, resetCollision);
        }

        private static void FreezePlayerWorkaroundMethod(object[] args)
        {
            bool freeze = (bool)args[0];
            Player.LocalPlayer.FreezePosition(freeze);
        }

        private static void SetEntityCollisionlessWorkaroundMethod(object[] args)
        {
            EntityCollisionlessInfoDto info = JsonConvert.DeserializeObject<EntityCollisionlessInfoDto>(args[0].ToString());
            info.EntityValue = Entities.Objects.GetAtRemote((ushort)info.EntityValue).Handle;
            RAGE.Game.Entity.SetEntityCollision(info.EntityValue, !info.Collisionless, true);
        }

        private static void SetPlayerTeamWorkaroundMethod(object[] args)
        {
            int team = (int)args[0];
            RAGE.Game.Player.SetPlayerTeam(team);
        }

        private static void UnspectatePlayerWorkaroundMethod(object[] args)
        {
#warning Add unspectatePlayer workaround (need a spectate system for this)
        }
    }
}
