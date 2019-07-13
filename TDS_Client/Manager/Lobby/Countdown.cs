using RAGE.NUI;
using System;
using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Common.Instance.Utility;

namespace TDS_Client.Manager.Lobby
{
    internal static class Countdown
    {
        private static DxText text;
        private static TDSTimer countdownTimer;
        private static int currentCountdownTime;

        private static string[] countdownSounds = new string[] { "go", "1", "2", "3" };

        public static void Start()
        {
            if (Settings.CountdownTime == 0)
            {
                End();
                return;
            }
            countdownTimer?.Kill();
            currentCountdownTime = Settings.CountdownTime;
            countdownTimer = new TDSTimer(Refresh, 1000, (uint)currentCountdownTime);
            text = new DxText(currentCountdownTime.ToString(), 0.5f, 0.2f, 2f, Color.White, alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Center);
            text.BlendScale(6f, 1000);
            PlaySound();
        }

        public static void StartAfterwards(int timeremainingms)
        {
            currentCountdownTime = (int)Math.Ceiling((double)timeremainingms / 1000);
            countdownTimer?.Kill();
            countdownTimer = new TDSTimer(() =>
            {
                if (currentCountdownTime > 1)
                    countdownTimer = new TDSTimer(Refresh, 1000, (uint)(currentCountdownTime - 1));
                Refresh();
            }, (uint)(currentCountdownTime - timeremainingms), 1);
            text = new DxText(currentCountdownTime.ToString(), 0.5f, 0.2f, 2f, Color.White, alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Center);
            text.BlendScale(6f, 1000);
            PlaySound();
        }

        public static void End()
        {
            if (text == null)
                text = new DxText("GO", 0.5f, 0.2f, 2f, Color.White, alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Center);
            else
            {
                text.Text = "GO";
                text.SetScale(2f);
            }

            countdownTimer?.Kill();
            currentCountdownTime = 0;
            PlaySound();
            countdownTimer = new TDSTimer(Stop, 2000, 1);
        }

        public static void Stop()
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
            if (text == null)
                return;
            text.Text = currentCountdownTime.ToString();
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