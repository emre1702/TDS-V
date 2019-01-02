using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Default;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Instance.Utility;

namespace TDS_Client.Manager.Lobby
{
    static class Bomb
    {
        private static bool dataChanged;
        private static Vector3 plantedPos;
        private static EPlantDefuseStatus playerStatus;
        private static bool gotBomb;
        private static List<Vector3> plantSpots;
        private static ulong plantDefuseStartTick;

        private static DxProgressRectangle progressRect;

        public static bool CheckPlantDefuseOnTick { get; private set; }

        public static void Detonate()
        {
            Cam.ShakeGameplayCam(DShakeName.LARGE_EXPLOSION_SHAKE, 1.0f);
            new TDSTimer(() => Cam.StopGameplayCamShaking(true), 4000, 1);
        }

        public static void BombPlanted(Vector3 pos, bool candefuse)
        {
            if (candefuse)
            {
                dataChanged = true;
                plantedPos = pos;
                CheckPlantDefuseOnTick = true;
            }
            RoundInfo.SetRoundTimeLeft((ulong)Settings.BombDetonateTimeMs);
        }

        public static void CheckPlantDefuse()
        {
            if (playerStatus == EPlantDefuseStatus.None)
                CheckPlantDefuseStart();
            else
                CheckPlantDefuseStop();                                    
        }

        public static void LocalPlayerGotBomb(List<Vector3> spotstoplant)
        {
            dataChanged = true;
            gotBomb = true;
            plantSpots = spotstoplant;
            CheckPlantDefuseOnTick = true;
        }

        public static void LocalPlayerPlantedBomb()
        {
            gotBomb = false;
            playerStatus = EPlantDefuseStatus.None;
            CheckPlantDefuseOnTick = false;
            progressRect.Remove();
            progressRect = null;
        }

        private static void UpdatePlantDefuseProgress()
        {
            Pad.DisableControlAction(0, (int)Control.Attack, true);
            ulong mswasted = TimerManager.ElapsedTicks - plantDefuseStartTick;
            uint mstoplantordefuse = Settings.GetPlantOrDefuseTime(playerStatus);
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
            if (!Pad.IsControlPressed(0, (int)Control.Attack))
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
                    Events.CallRemote(DToServerEvent.StopPlanting);
                else if (playerStatus == EPlantDefuseStatus.Defusing)
                    Events.CallRemote(DToServerEvent.StopDefusing);
                playerStatus = EPlantDefuseStatus.None;
                progressRect.Remove();
                progressRect = null;
            } 
            else
                UpdatePlantDefuseProgress();
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
            progressRect = new DxProgressRectangle(Settings.Language.PLANTING, 0.5f, 0.71f, 0.08f, 0.02f, Color.White, Color.Black, Color.ForestGreen, alignment: Alignment.Center);
            Events.CallRemote(DToServerEvent.StartPlanting);
        }

        private static void CheckDefuseStart()
        {
            if (!IsOnDefuseSpot())
                return;
            plantDefuseStartTick = TimerManager.ElapsedTicks;
            playerStatus = EPlantDefuseStatus.Defusing;
            progressRect = new DxProgressRectangle(Settings.Language.DEFUSING, 0.5f, 0.71f, 0.08f, 0.02f, Color.White, Color.Black, Color.ForestGreen, alignment: Alignment.Center);
            Events.CallRemote(DToServerEvent.StartDefusing);
        }

        private static bool IsOnPlantSpot()
        {
            Vector3 playerpos = RAGE.Elements.Player.LocalPlayer.Position;
            foreach (Vector3 pos in plantSpots)
            {
                if (Misc.GetDistanceBetweenCoords(playerpos.X, playerpos.Y, playerpos.Z, pos.X, pos.Y, pos.Z, true) <= Settings.DistanceToSpotToPlant)
                    return true;
            }
            return false;
        }

        private static bool IsOnDefuseSpot()
        {
            Vector3 playerpos = RAGE.Elements.Player.LocalPlayer.Position;
            return Misc.GetDistanceBetweenCoords(playerpos.X, playerpos.Y, playerpos.Z, plantedPos.X, plantedPos.Y, plantedPos.Z, true) <= Settings.DistanceToSpotToDefuse;
        }

        public static void Reset()
        {
            if (!dataChanged)
                return;
            dataChanged = false;
            CheckPlantDefuseOnTick = false;
            progressRect.Remove();
            progressRect = null;
            gotBomb = false;
            plantSpots = null;
            playerStatus = EPlantDefuseStatus.None;
            plantDefuseStartTick = 0;
            plantedPos = null;
        }
    }
}
