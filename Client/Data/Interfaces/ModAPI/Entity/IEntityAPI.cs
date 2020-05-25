using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Entity
{
    public interface IEntityAPI
    {
        /**
         * <summary>
         * Attaches entity1 to bone(boneIndex) of entity2.boneIndex - this is different
         * to boneID, use GET_PED_BONE_INDEX to get the index from the ID.
         * use the index for attaching to specific bones.
         * entity1 will be attached to entity2s centre if bone index given doesnt correspond to bone indexes for that entity type.
         * useSoftPinning - if set to false attached entity will not detach when fixed
         * collision - controls collision between the two entities(FALSE disables collision).
         * isPed - pitch doesnt work when false and roll will only work on negative numbers(only peds)
         * vertexIndex - position of vertex
         * fixedRot - if false it ignores entity vector
         * </summary>
         */

        #region Public Methods

        void AttachEntityToEntity(int sourceEntity, int targetEntity, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot,
            bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot);

        void DetachEntity(int entity);

        Position3D GetEntityCoords(int entity, bool alive);

        int GetEntityHealth(int entity);

        float GetEntityHeightAboveGround(int entity);

        EntityType GetEntityType(int entity);

        Position3D GetOffsetFromEntityInWorldCoords(int entity, float offsetX, float offsetY, float offsetZ);

        void SetEntityCollision(int entity, bool toggle, bool keepPhysics);

        void SetEntityCoordsNoOffset(int entity, float xPos, float yPos, float zPos, bool xAxis, bool yAxis, bool zAxis);

        void SetEntityInvincible(int entity, bool toggle);

        #endregion Public Methods

        /**
         * <summary>
         * Only works for Objects, Vehicles and Peds (and Players?)
         * </summary>
         */
    }
}
