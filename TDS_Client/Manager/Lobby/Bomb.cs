using RAGE;
using RAGE.Game;
using RAGE.NUI;
using System.Drawing;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto.Map;
using TDS_Common.Enum;
using TDS_Common.Instance.Utility;

namespace TDS_Client.Manager.Lobby
{
    internal static class Bomb
    {
        private static Vector3 plantedPos;
        private static EPlantDefuseStatus playerStatus;
        private static bool gotBomb;
        private static bool bombPlanted;
        private static MapPositionDto[] plantSpots;
        private static ulong plantDefuseStartTick;

        private static DxProgressRectangle progressRect;

        public static bool DataChanged;
        public static bool CheckPlantDefuseOnTick { get; private set; }
        public static bool BombOnHand { get; set; }

        public static void Detonate()
        {
            Cam.ShakeGameplayCam(DShakeName.LARGE_EXPLOSION_SHAKE, 1.0f);
            new TDSTimer(() => Cam.StopGameplayCamShaking(true), 4000, 1);
            MainBrowser.StopBombTick();
        }

        public static void BombPlanted(Vector3 pos, bool candefuse, int? startAtMs)
        {
            DataChanged = true;
            if (candefuse)
            {
                plantedPos = pos;
                CheckPlantDefuseOnTick = true;
            }
            bombPlanted = true;
            if (startAtMs.HasValue)
            {
                startAtMs += 100;  // 100 because trigger etc. propably took some time
                RoundInfo.SetRoundTimeLeft(Settings.BombDetonateTimeMs - startAtMs.Value);
                MainBrowser.StartBombTick(Settings.BombDetonateTimeMs, startAtMs.Value);
            }
            else
            {
                RoundInfo.SetRoundTimeLeft(Settings.BombDetonateTimeMs);
                MainBrowser.StartBombTick(Settings.BombDetonateTimeMs, 0);
            }
            ClientUtils.Notify(Settings.Language.BOMB_PLANTED);
        }

        public static void CheckPlantDefuse()
        {
            if (playerStatus == EPlantDefuseStatus.None)
                CheckPlantDefuseStart();
            else
                CheckPlantDefuseStop();
        }

        public static void LocalPlayerGotBomb(MapPositionDto[] spotstoplant)
        {
            DataChanged = true;
            gotBomb = true;
            plantSpots = spotstoplant;
            CheckPlantDefuseOnTick = true;
        }

        public static void LocalPlayerPlantedBomb()
        {
            gotBomb = false;
            playerStatus = EPlantDefuseStatus.None;
            CheckPlantDefuseOnTick = false;
            progressRect?.Remove();
            progressRect = null;
            BombOnHand = false;
        }

        public static void UpdatePlantDefuseProgress()
        {
            if (playerStatus == EPlantDefuseStatus.None)
                return;
            if (progressRect == null)
                return;
            ulong mswasted = TimerManager.ElapsedTicks - plantDefuseStartTick;
            float mstoplantordefuse = Settings.GetPlantOrDefuseTime(playerStatus);
            if (mswasted < mstoplantordefuse)
            {
                float progress = mswasted / mstoplantordefuse;
                progressRect.Progress = progress;
            }
            else
            {
                progressRect.Progress = 1;
            }
        }

        private static void CheckPlantDefuseStart()
        {
            if (!Pad.IsDisabledControlPressed(0, (int)Control.Attack))
                return;
            if (gotBomb)
                CheckPlantStart();
            else
                CheckDefuseStart();
        }

        private static void CheckPlantDefuseStop()
        {
            if (ShouldPlantDefuseStop())
            {
                if (playerStatus == EPlantDefuseStatus.Planting)
                    EventsSender.Send(DToServerEvent.StopPlanting);
                else if (playerStatus == EPlantDefuseStatus.Defusing)
                    EventsSender.Send(DToServerEvent.StopDefusing);
                playerStatus = EPlantDefuseStatus.None;
                progressRect?.Remove();
                progressRect = null;
            }
        }

        private static bool ShouldPlantDefuseStop()
        {
            uint weaponHash = RAGE.Elements.Player.LocalPlayer.GetSelectedWeapon();
            if (weaponHash != (uint)EWeaponHash.Unarmed)
                return true;
            if (!Pad.IsDisabledControlPressed(0, (int)Control.Attack))
                return true;
            if (RAGE.Elements.Player.LocalPlayer.IsDeadOrDying(true))
                return true;
            return false;
        }

        private static void CheckPlantStart()
        {
            if (!IsOnPlantSpot())
                return;
            plantDefuseStartTick = TimerManager.ElapsedTicks;
            playerStatus = EPlantDefuseStatus.Planting;
            progressRect = new DxProgressRectangle(Settings.Language.PLANTING, 0.5f, 0.71f, 0.12f, 0.05f, Color.White, Color.Black, Color.ForestGreen, textScale: 0.7f,
                alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Center);
            EventsSender.Send(DToServerEvent.StartPlanting);
        }

        private static void CheckDefuseStart()
        {
            if (!IsOnDefuseSpot())
                return;
            plantDefuseStartTick = TimerManager.ElapsedTicks;
            playerStatus = EPlantDefuseStatus.Defusing;
            progressRect = new DxProgressRectangle(Settings.Language.DEFUSING, 0.5f, 0.71f, 0.12f, 0.05f, Color.White, Color.Black, Color.ForestGreen, textScale: 0.7f,
                alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Center);
            EventsSender.Send(DToServerEvent.StartDefusing);
        }

        private static bool IsOnPlantSpot()
        {
            if (plantSpots == null)
                return false;
            Vector3 playerpos = RAGE.Elements.Player.LocalPlayer.Position;
            foreach (MapPositionDto pos in plantSpots)
            {
                if (Misc.GetDistanceBetweenCoords(playerpos.X, playerpos.Y, playerpos.Z, pos.X, pos.Y, pos.Z, pos.Z != 0) <= Settings.DistanceToSpotToPlant)
                    return true;
            }
            return false;
        }

        private static bool IsOnDefuseSpot()
        {
            if (plantedPos == null)
                return false;
            Vector3 playerpos = RAGE.Elements.Player.LocalPlayer.Position;
            return Misc.GetDistanceBetweenCoords(playerpos.X, playerpos.Y, playerpos.Z, plantedPos.X, plantedPos.Y, plantedPos.Z, true) <= Settings.DistanceToSpotToDefuse;
        }

        public static void Reset()
        {
            if (!DataChanged)
                return;
            DataChanged = false;
            BombOnHand = false;
            CheckPlantDefuseOnTick = false;
            progressRect?.Remove();
            progressRect = null;
            gotBomb = false;
            plantSpots = null;
            playerStatus = EPlantDefuseStatus.None;
            plantDefuseStartTick = 0;
            if (bombPlanted)
                MainBrowser.StopBombTick();
            bombPlanted = false;
            plantedPos = null;
        }
    }
}