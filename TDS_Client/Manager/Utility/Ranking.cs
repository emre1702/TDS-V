using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    class Ranking
    {
        private static Player _winner = null;
        private static Player _second = null;
        private static Player _third = null;

        public static void Start(string rankingsJson, ushort winnerHandle, ushort secondHandle, ushort thirdHandle)
        {
            Death.PlayerSpawn();

            var cam = CameraManager.BetweenRoundsCam;
            cam.Position = new Vector3(-425.2233f, 1126.9731f, 326.8f);
            cam.PointCamAtCoord(new Vector3(-427.03f, 1123.21f, 325.85f));
            cam.Activate(true);
            TDSCamera.SetFocusArea(-425.2233f, 1126.9731f, 326.8f);
            // Cam-pos:
            //X: -425,2233
            //Y: 1126.9731
            //Z: 326.8
            //Rot: 160

            Cam.DoScreenFadeIn(200);

            if (Settings.PlayerSettings.ShowConfettiAtRanking) 
                TickManager.Add(OnRender);

            _winner = ClientUtils.GetPlayerByHandleValue(winnerHandle);
            _second = secondHandle != 0 ? ClientUtils.GetPlayerByHandleValue(secondHandle) : null;
            _third = thirdHandle != 0 ? ClientUtils.GetPlayerByHandleValue(thirdHandle) : null;

            Browser.Angular.Main.ShowRankings(rankingsJson);
            CursorManager.Visible = true;
        }

        public static void Stop()
        {
            TickManager.Remove(OnRender);
            TDSCamera.RemoveFocusArea();
            Browser.Angular.Main.HideRankings();
            CursorManager.Visible = false;
        }

        private static void OnRender()
        {
            //StartParticleFx("scr_xs_money_rain", -425.48f, 1123.55f, 325.85f, 1f);
            //StartParticleFx("scr_xs_money_rain_celeb", 427.03f, 1123.21f, 325.85f, 1f);

            StartParticleFx("scr_xs_confetti_burst", -428.01f, 1123.47f, 325f, 1.5f);
            StartParticleFx("scr_xs_confetti_burst", -423.48f, 1122.09f, 325f, 1.5f);
            StartParticleFx("scr_xs_confetti_burst", -426.17f, 1121.18f, 325f, 2f);

            // didnt work
            //StartParticleFx("scr_xs_champagne_spray", -428.01f, 1123.47f, 325f, 1.5f);
            //StartParticleFx("scr_xs_champagne_spray", -423.48f, 1122.09f, 325f, 1.5f);
            //StartParticleFx("scr_xs_beer_chug", -426.17f, 1121.18f, 325f, 2f);

            if (!(_winner is null) && _winner.Exists) 
                Nametag.DrawNametag(_winner.Handle, "1. " + _winner.GetDisplayName(), 5f);
            if (!(_second is null) && _second.Exists)
                Nametag.DrawNametag(_second.Handle, "2. " + _second.GetDisplayName(), 5f);
            if (!(_third is null) && _third.Exists)
                Nametag.DrawNametag(_third.Handle, "3. " + _third.GetDisplayName(), 5f);

            //StartParticleFx("scr_xs_champagne_spray", -425.48f, 1123.55f, 325.85f, 1f);
            //StartParticleFx("scr_xs_beer_chug", 427.03f, 1123.21f, 325.85f, 1f);
        }

        private static int StartParticleFx(string effectName, float x, float y, float z, float scale)
        {
            Graphics.UseParticleFxAssetNextCall("scr_xs_celebration");
            return Graphics.StartParticleFxNonLoopedAtCoord(effectName, x, y, z, 0, 0, 0, scale, false, false, false);
        }
    }
}
