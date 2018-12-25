using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Common.Instance.Utility;

namespace TDS_Client.Manager.Lobby
{
    static class Countdown
    {
        private static DxText text;
        private static TDSTimer countdownTimer;
        private static uint currentCountdownTime;

        private static string[] countdownSounds = new string[] { "go", "1", "2", "3" };

        public static void Start()
        {
            if (Settings.CountdownTime == 0)
            {
                End();
                return;
            }
            countdownTimer?.Kill();
            countdownTimer = new TDSTimer(Refresh, 1000, currentCountdownTime);
            currentCountdownTime = Settings.CountdownTime;
            text = new DxText(currentCountdownTime.ToString(), 0.5f, 0.2f, 2f, Color.White, alignment: Alignment.Center);
            text.BlendScale(6f, 1000);
            PlaySound();
        }

        public static void StartAfterwards(uint timeremainingms)
        {
            currentCountdownTime = (uint)Math.Ceiling((double)timeremainingms / 1000);
            countdownTimer?.Kill();
            countdownTimer = new TDSTimer(() =>
            {
                if (currentCountdownTime > 1)
                    countdownTimer = new TDSTimer(Refresh, 1000, currentCountdownTime-1);
                Refresh();
            }, currentCountdownTime - timeremainingms, 1);
            text = new DxText(currentCountdownTime.ToString(), 0.5f, 0.2f, 2f, Color.White, alignment: Alignment.Center);
            text.BlendScale(6f, 1000);
            PlaySound();
        }

        private static void End()
        {
            if (text == null)
                text = new DxText("GO", 0.5f, 0.2f, 2f, Color.White, alignment: Alignment.Center);
            else
                text.SetText("GO");
            countdownTimer?.Kill();
            currentCountdownTime = 0;
            PlaySound();
            countdownTimer = new TDSTimer(Stop, 2000, 1);
        }

        private static void Stop()
        {
            text?.Remove();
            text = null;
            countdownTimer?.Kill();
            countdownTimer = null;
        }

        private static void Refresh()
        {
            if (--currentCountdownTime <= 0)
            {
                End();
                return;
            }
            text.SetText(currentCountdownTime.ToString());
            text.SetScale(2f);
            text.BlendScale(6f, 1000);
            PlaySound();
        }

        private static void PlaySound()
        {
            if (countdownSounds.Length > currentCountdownTime)
                MainBrowser.PlaySound(countdownSounds[currentCountdownTime]);
        }
    }
}
