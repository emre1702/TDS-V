using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Entity
{
    internal class EntityAPI : IEntityAPI
    {
        #region Public Methods

        public void AttachEntityToEntity(int sourceEntity, int targetEntity, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot,
            bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
        {
            RAGE.Game.Entity.AttachEntityToEntity(sourceEntity, targetEntity, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
        }

        public void DetachEntity(int entity)
        {
            RAGE.Game.Entity.DetachEntity(entity, true, true);
        }

        public Position3D GetEntityCoords(int entity, bool alive)
        {
            return RAGE.Game.Entity.GetEntityCoords(entity, alive).ToPosition3D();
        }

        public int GetEntityHealth(int entity)
        {
            return RAGE.Game.Entity.GetEntityHealth(entity);
        }

        public float GetEntityHeightAboveGround(int entity)
        {
            return RAGE.Game.Entity.GetEntityHeightAboveGround(entity);
        }

        public EntityType GetEntityType(int entity)
        {
            var type = RAGE.Game.Entity.GetEntityType(entity);

            switch (type)
            {
                case 3:
                    return EntityType.Object;

                case 2:
                    return EntityType.Vehicle;

                case 1:
                    return EntityType.Ped;

                default:
                    return EntityType.Invalid;
            }
        }

        public Position3D GetOffsetFromEntityInWorldCoords(int entity, float offsetX, float offsetY, float offsetZ)
        {
            return RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(entity, offsetX, offsetY, offsetZ).ToPosition3D();
        }

        public void SetEntityCollision(int entity, bool toggle, bool keepPhysics)
        {
            RAGE.Game.Entity.SetEntityCollision(entity, toggle, keepPhysics);
        }

        public void SetEntityCoordsNoOffset(int entity, float xPos, float yPos, float zPos, bool xAxis, bool yAxis, bool zAxis)
        {
            RAGE.Game.Entity.SetEntityCoordsNoOffset(entity, xPos, yPos, zPos, xAxis, yAxis, zAxis);
        }

        public void SetEntityInvincible(int entity, bool toggle)
        {
            RAGE.Game.Entity.SetEntityInvincible(entity, toggle);
        }

        #endregion Public Methods
    }
}
