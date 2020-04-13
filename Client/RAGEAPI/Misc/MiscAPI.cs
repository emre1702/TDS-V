using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Misc;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Misc
{
    class MiscAPI : IMiscAPI
    {
        public float GetDistanceBetweenCoords(Position3D pos1, Position3D pos2, bool useZ)
        {
            return RAGE.Game.Misc.GetDistanceBetweenCoords(pos1.X, pos1.Y, pos1.Z, pos2.X, pos2.Y, pos2.Z, useZ);
        }

        public bool GetGroundZFor3dCoord(float x, float y, float z, ref float groundZ)
        {
            return RAGE.Game.Misc.GetGroundZFor3dCoord(x, y, z, ref groundZ, false);
        }

        public uint GetHashKey(string hash)
        {
            return RAGE.Game.Misc.GetHashKey(hash);
        }

        public void GetModelDimensions(uint modelHash, Position3D a, Position3D b)
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

        public int GetGameTimer()
        {
            return RAGE.Game.Misc.GetGameTimer();
        }
    }
}
