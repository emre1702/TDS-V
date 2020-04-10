using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Entity
{
    public interface IEntity : IEquatable<IEntity>
    {
        int Handle { get; }
        Position3D Position { get; set; }
        Position3D Rotation { get; set; }
        int Dimension { get; set; }
        bool IsNull { get; } // Oder Exists
        int Alpha { get; set; }
        EntityType Type { get; }

        void FreezePosition(bool freeze);
        void GetModelDimension(Position3D a, Position3D b);
        void ActivatePhysics();
        void Destroy();
        void SetInvincible(bool toggle);
        Position3D GetOffsetInWorldCoords(float v1, float v2, float v3);
        void SetCollision(bool v1, bool v2);
    }
}
