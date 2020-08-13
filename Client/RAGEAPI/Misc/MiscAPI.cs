using TDS_Client.Data.Interfaces.ModAPI.Misc;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Misc
{
    internal class MiscAPI : IMiscAPI
    {
        #region Public Methods

        public float GetDistanceBetweenCoords(Position pos1, Position pos2, bool useZ)
        {
            return RAGE.Game.Misc.GetDistanceBetweenCoords(pos1.X, pos1.Y, pos1.Z, pos2.X, pos2.Y, pos2.Z, useZ);
        }

        public int GetGameTimer()
        {
            return RAGE.Game.Misc.GetGameTimer();
        }

        public bool GetGroundZFor3dCoord(float x, float y, float z, ref float groundZ)
        {
            return RAGE.Game.Misc.GetGroundZFor3dCoord(x, y, z, ref groundZ, false);
        }

        public uint GetHashKey(string hash)
        {
            return RAGE.Game.Misc.GetHashKey(hash);
        }

        public void GetModelDimensions(uint modelHash, Position a, Position b)
        {
            var aV = a.ToVector3();
            var bV = b.ToVector3();
            RAGE.Game.Misc.GetModelDimensions(modelHash, aV, bV);

            a.X = aV.X;
            a.Y = aV.Y;
            a.Z = aV.Z;
            b.X = bV.X;
            b.Y = bV.Y;
            b.Z = bV.Z;
        }

        public void IgnoreNextRestart(bool toggle)
        {
            RAGE.Game.Misc.IgnoreNextRestart(toggle);
        }

        public void SetFadeOutAfterDeath(bool toggle)
        {
            RAGE.Game.Misc.SetFadeOutAfterDeath(toggle);
        }

        public void SetWeatherTypeNowPersist(string weatherType)
        {
            RAGE.Game.Misc.SetWeatherTypeNowPersist(weatherType);
        }

        public void SetWind(float speed)
        {
            RAGE.Game.Misc.SetWind(speed);
        }

        public void SetGravityLevel(int level) 
            => RAGE.Game.Misc.SetGravityLevel(level);

        public void SetExplosiveAmmoThisFrame(IPlayer player)
            => RAGE.Game.Misc.SetExplosiveAmmoThisFrame(player.Handle);

        public void SetExplosiveMeleeThisFrame(IPlayer player)
            => RAGE.Game.Misc.SetExplosiveMeleeThisFrame(player.Handle);

        public void SetFireAmmoThisFrame(IPlayer player)
            => RAGE.Game.Misc.SetFireAmmoThisFrame(player.Handle);

        public void SetSuperJumpThisFrame(IPlayer player)
            => RAGE.Game.Misc.SetSuperJumpThisFrame(player.Handle);


        #endregion Public Methods
    }
}
