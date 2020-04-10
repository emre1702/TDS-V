using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Entity
{
    public interface IEntityAPI
    {
        void AttachEntityToEntity(int entityValue, int targetValue, int bone, float positionOffsetX, float positionOffsetY, float positionOffsetZ, float rotationOffsetX, float rotationOffsetY, float rotationOffsetZ, bool v1, bool v2, bool v3, bool v4, int v5, bool v6);
        void DetachEntity(int entity, bool v, bool resetCollision);
        void SetEntityCollision(int entityValue, bool v1, bool v2);
        void SetEntityInvincible(object value, bool toggle);
        /*
         *         None = 0,
                    Ped = 1,
                    Vehicle = 2,
                    Object = 3

            to EntityType

         * */
        EntityType GetEntityType(int targetEntity);

        Position3D GetEntityCoords(int targetEntity, bool v);
        int GetEntityHealth(int handle);
        void SetEntityCoordsNoOffset(int handle, float x, float y, float z, bool v1, bool v2, bool v3);
        Position3D GetOffsetFromEntityInWorldCoords(int handle, float v1, float v2, float v3);
        float GetEntityHeightAboveGround(int handle);
    }
}
