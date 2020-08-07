using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Core.Init
{
    public class Program
    {
        #region Public Constructors

        public Program(IModAPI modAPI)
        {
            Init(modAPI);
            Services.Initialize(modAPI);
        }

        #endregion Public Constructors

        #region Private Methods

        private void Init(IModAPI modAPI)
        {
            modAPI.Streaming.RequestNamedPtfxAsset("scr_xs_celebration");
            modAPI.Misc.SetWeatherTypeNowPersist("CLEAR");
            modAPI.Misc.SetWind(0);
            modAPI.Streaming.RequestAnimDict("MP_SUICIDE");
            modAPI.Audio.SetAudioFlag("LoadMPData", true);
            modAPI.Player.SetPlayerHealthRechargeMultiplier(0);
            modAPI.LocalPlayer.SetCanRagdoll(false);

            // CLEAR_GPS_CUSTOM_ROUTE
            modAPI.Native.Invoke(TDS_Shared.Data.Enums.NativeHash.CLEAR_GPS_CUSTOM_ROUTE);
        }

        #endregion Private Methods
    }
}
