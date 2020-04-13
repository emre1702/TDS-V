using System;
using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Entity
{
    public interface IEntityBase : IEntity, IEquatable<IEntityBase>
    {
        Position3D Rotation { get; set; }
        float Heading { get; set; }
        int Alpha { get; set; }
        int Health { get; set; }
        

        void FreezePosition(bool freeze);
        void GetModelDimensions(Position3D a, Position3D b);
        void ActivatePhysics();
        void SetInvincible(bool toggle);
        Position3D GetOffsetInWorldCoords(float offsetX, float offsetY, float offsetZ);
        void SetCollision(bool toggle, bool keepPhysics);
        int AddBlipFor();
        int GetBlipFrom();
        void ClearLastDamageEntity();
        float GetAnimCurrentTime(string animDict, string animName);
        bool HasAnimEventFired(uint actionHash);
        bool IsPlayingAnim(string animDict, string animName);
        void ResetAlpha();
        void SetVisible(bool toggle);
    }
}
