using RAGE;
using RAGE.Game;
using RAGE.NUI;
using System;
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
        private static Vector3 _plantedPos;
        private static EPlantDefuseStatus _playerStatus;
        private static bool _gotBomb;
        private static bool _bombPlanted;
        private static Position4DDto[] _plantSpots;
        private static ulong _plantDefuseStartTick;

        private static DxProgressRectangle _progressRect;

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
                _plantedPos = pos;
                CheckPlantDefuseOnTick = true;
            }
            _bombPlanted = true;
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
            if (_playerStatus == EPlantDefuseStatus.None)
                CheckPlantDefuseStart();
            else
                CheckPlantDefuseStop();
        }

        public static void LocalPlayerGotBomb(Position4DDto[] spotstoplant)
        {
            DataChanged = true;
            _gotBomb = true;
            _plantSpots = spotstoplant;
            CheckPlantDefuseOnTick = true;
        }

        public static void LocalPlayerPlantedBomb()
        {
            _gotBomb = false;
            _playerStatus = EPlantDefuseStatus.None;
            CheckPlantDefuseOnTick = false;
            _progressRect?.Remove();
            _progressRect = null;
            BombOnHand = false;
        }

        public static void UpdatePlantDefuseProgress()
        {
            if (_playerStatus == EPlantDefuseStatus.None)
                return;
            if (_progressRect == null)
                return;
            ulong mswasted = TimerManager.ElapsedTicks - _plantDefuseStartTick;
            float mstoplantordefuse = Settings.GetPlantOrDefuseTime(_playerStatus);
            _progressRect.Progress = Math.Min(mswasted / mstoplantordefuse, 1);
        }

        private static void CheckPlantDefuseStart()
        {
            if (!Pad.IsDisabledControlPressed(0, (int)Control.Attack))
                return;
            if (_gotBomb)
                CheckPlantStart();
            else
                CheckDefuseStart();
        }

        private static void CheckPlantDefuseStop()
        {
            if (ShouldPlantDefuseStop())
            {
                if (_playerStatus == EPlantDefuseStatus.Planting)
                    EventsSender.Send(DToServerEvent.StopPlanting);
                else if (_playerStatus == EPlantDefuseStatus.Defusing)
                    EventsSender.Send(DToServerEvent.StopDefusing);
                _playerStatus = EPlantDefuseStatus.None;
                _progressRect?.Remove();
                _progressRect = null;
            }
        }

        public static void StopRequestByServer()
        {
            _playerStatus = EPlantDefuseStatus.None;
            _progressRect?.Remove();
            _progressRect = null;
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
            _plantDefuseStartTick = TimerManager.ElapsedTicks;
            _playerStatus = EPlantDefuseStatus.Planting;
            _progressRect = new DxProgressRectangle(Settings.Language.PLANTING, 0.5f, 0.71f, 0.12f, 0.05f, Color.White, Color.Black, Color.ForestGreen, textScale: 0.7f,
                alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Center);
            EventsSender.Send(DToServerEvent.StartPlanting);
        }

        private static void CheckDefuseStart()
        {
            if (!IsOnDefuseSpot())
                return;
            _plantDefuseStartTick = TimerManager.ElapsedTicks;
            _playerStatus = EPlantDefuseStatus.Defusing;
            _progressRect = new DxProgressRectangle(Settings.Language.DEFUSING, 0.5f, 0.71f, 0.12f, 0.05f, Color.White, Color.Black, Color.ForestGreen, textScale: 0.7f,
                alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Center);
            EventsSender.Send(DToServerEvent.StartDefusing);
        }

        private static bool IsOnPlantSpot()
        {
            if (_plantSpots == null)
                return false;
            Vector3 playerpos = RAGE.Elements.Player.LocalPlayer.Position;
            foreach (Position4DDto pos in _plantSpots)
            {
                if (Misc.GetDistanceBetweenCoords(playerpos.X, playerpos.Y, playerpos.Z, pos.X, pos.Y, pos.Z, pos.Z != 0) <= Settings.DistanceToSpotToPlant)
                    return true;
            }
            return false;
        }

        private static bool IsOnDefuseSpot()
        {
            if (_plantedPos == null)
                return false;
            Vector3 playerpos = RAGE.Elements.Player.LocalPlayer.Position;
            return Misc.GetDistanceBetweenCoords(playerpos.X, playerpos.Y, playerpos.Z, _plantedPos.X, _plantedPos.Y, _plantedPos.Z, true) <= Settings.DistanceToSpotToDefuse;
        }

        public static void Reset()
        {
            BombOnHand = false;
            if (!DataChanged)
                return;
            DataChanged = false;
            CheckPlantDefuseOnTick = false;
            _progressRect?.Remove();
            _progressRect = null;
            _gotBomb = false;
            _plantSpots = null;
            _playerStatus = EPlantDefuseStatus.None;
            _plantDefuseStartTick = 0;
            if (_bombPlanted)
                MainBrowser.StopBombTick();
            _bombPlanted = false;
            _plantedPos = null;
        }
    }
}