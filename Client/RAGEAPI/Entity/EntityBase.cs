using System;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Entity
{
    class EntityBase : Entity, IEntityBase
    {
        private readonly RAGE.Elements.GameEntityBase _instance;

        public EntityBase(RAGE.Elements.GameEntityBase instance) : base(instance)
        {
            _instance = instance;
        }

        public Position3D Rotation
        {
            get => _instance.GetRotation(2).ToPosition3D();
            set => _instance.SetRotation(value.X, value.Y, value.Z, 2, true);
        }
        public int Alpha
        {
            get => _instance.GetAlpha();
            set => _instance.SetAlpha(value, false);
        }
        public float Heading
        {
            get => _instance.GetHeading();
            set => _instance.SetHeading(value);
        }

        public int Health 
        { 
            get => _instance.GetHealth(); 
            set => _instance.SetHealth(value); 
        }

        public void ActivatePhysics()
        {
            _instance.ActivatePhysics();
        }


        public bool Equals(IEntityBase other)
        {
            return Handle == other.Handle;
        }

        public void FreezePosition(bool freeze)
        {
            _instance.FreezePosition(freeze);
        }

        public void GetModelDimensions(Position3D a, Position3D b)
        {
            var aV = a.ToVector3();
            var bV = b.ToVector3();
            RAGE.Game.Misc.GetModelDimensions(_instance.Model, aV, bV);
            
            a.X = aV.X;
            a.Y = aV.Y;
            a.Z = aV.Z;
            b.X = bV.X;
            b.Y = bV.Y;
            b.Z = bV.Z;
        }

        public Position3D GetOffsetInWorldCoords(float offsetX, float offsetY, float offsetZ)
        {
            return RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(_instance.Handle, offsetX, offsetY, offsetZ).ToPosition3D();
        }

        public void SetCollision(bool toggle, bool keepPhysics)
        {
            RAGE.Game.Entity.SetEntityCollision(_instance.Handle, toggle, keepPhysics);
        }

        public void SetInvincible(bool toggle)
        {
            RAGE.Game.Entity.SetEntityInvincible(_instance.Handle, toggle);
        }

        public int AddBlipFor()
        {
            return _instance.AddBlipFor();
        }

        public int GetBlipFrom()
        {
            return RAGE.Game.Ui.GetBlipFromEntity(_instance.Handle);
        }

        public void ClearLastDamageEntity()
        {
            _instance.ClearLastDamageEntity();
        }

        public float GetAnimCurrentTime(string animDict, string animName)
        {
            return _instance.GetAnimCurrentTime(animDict, animName);
        }

        public bool HasAnimEventFired(uint actionHash)
        {
            return _instance.HasAnimEventFired(actionHash);
        }

        public bool IsPlayingAnim(string animDict, string animName)
        {
            return _instance.IsPlayingAnim(animDict, animName, 3);
        }

        public void ResetAlpha()
        {
            _instance.ResetAlpha();
        }

        public void SetVisible(bool toggle)
        {
            _instance.SetVisible(toggle, false);
        }
    }
}
