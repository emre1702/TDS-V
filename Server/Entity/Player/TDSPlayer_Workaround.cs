using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Public Methods

        public void SetEntityInvincible(ITDSVehicle vehicle, bool invincible)
        {
            vehicle.SetInvincible(invincible, this);
        }

        public void WarpOutOfVehicle()
        {
            //Todo: Implement
        }

        public void PlayAnimation(string dict, string name, int flag)
        {
            //Todo: Implement
        }

        public void Freeze(bool toggle)
        {
            //Todo: Implement
        }

        public void SetSkin(PedHash pedHash)
        {
            //Todo: Implement
        }

        public void SetIntoVehicle(ITDSVehicle vehicle, sbyte seat = -2)
        {
            //Todo: Implement
        }


        public void SetEntityInvincible(IVehicle vehicle, bool invincible)
        {
            //Todo: Implement
        }

        public void StopAnimation()
        {
            //Todo: Implement
        }

        public void SetClothes(int slot, int drawable, int texture)
        {
            //Todo: Implement
        }

        public void SetInvincible(bool toggle)
        {
            //Todo: Implement
        }

        public void Detach()
        {
            //Todo: Implement
        }

        public void SetCollisionsless(bool toggle, ILobby forLobby)
        {
            //Todo: Implement
        }

        public void AttachTo(ITDSPlayer player, PedBone bone, Position? offsetPos, DegreeRotation? offsetRot)
        {
            //Todo: Implement
        }

        #endregion Public Methods
    }
}
