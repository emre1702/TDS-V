﻿using RAGE.Game;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Core.Init
{
    public class Program : RAGE.Events.Script
    {
        #region Public Constructors

        public Program()
        {
            Init();
            Services.Initialize();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Init()
        {
            Streaming.RequestNamedPtfxAsset("scr_xs_celebration");
            Misc.SetWeatherTypeNowPersist("CLEAR");
            Misc.SetWind(0);
            Streaming.RequestAnimDict("MP_SUICIDE");
            Audio.SetAudioFlag("LoadMPData", true);
            Player.SetPlayerHealthRechargeMultiplier(0);
            RAGE.Elements.Player.LocalPlayer.SetCanRagdoll(false);
            RAGE.Elements.Player.LocalPlayer.SetAlpha(255, true);
            RAGE.Elements.Player.LocalPlayer.SetVisible(true, true);

            // CLEAR_GPS_CUSTOM_ROUTE
            Invoker.Invoke((ulong)NativeHash.CLEAR_GPS_CUSTOM_ROUTE);
        }

        #endregion Private Methods
    }
}
