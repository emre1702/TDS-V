﻿using RAGE;
using RAGE.Game;
using RAGE.NUI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Utility;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Instance.Draw
{
    internal class FloatingDamageInfo
    {
        private static List<FloatingDamageInfo> damageInfos = new List<FloatingDamageInfo>();

        private float damage;
        private ulong startTicks;
        private Vector3 targetPosition;
        private DxText text;
        private bool remove = false;

        public FloatingDamageInfo(Player target, float damage)
        {
            this.damage = damage;
            startTicks = TimerManager.ElapsedTicks;
            targetPosition = target.Position;

            damageInfos.Add(this);
        }

        private void UpdatePosition()
        {
            int elapsedTicks = (int)(TimerManager.ElapsedTicks - startTicks);
            if (elapsedTicks > Settings.PlayerSettings.ShowFloatingDamageInfoDurationMs)
            {
                remove = true;
                Remove();
                return;
            }
            float screenX = 0;
            float screenY = 0;
            Graphics.GetScreenCoordFromWorldCoord(targetPosition.X, targetPosition.Y, targetPosition.Z + 1, ref screenX, ref screenY);

            float percentage = elapsedTicks / Settings.PlayerSettings.ShowFloatingDamageInfoDurationMs;
            screenY -= percentage * 0.05f;

            if (text == null)
                text = new DxText(damage.ToString(), screenX, screenY, 0.4f, Color.White, alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Bottom, dropShadow: true, outline: true);
            else
                text.SetRelativeY(screenY);
        }

        public static void UpdateAllPositions()
        {
            if (damageInfos.Count == 0)
                return;

            damageInfos.RemoveAll(x => x.remove);
            foreach (var damageInfo in damageInfos)
            {
                damageInfo.UpdatePosition();
            }
        }

        public static void RemoveAll()
        {
            foreach (var info in damageInfos)
            {
                info.Remove();
            }
            damageInfos = new List<FloatingDamageInfo>();
        }

        private void Remove()
        {
            text?.Remove();
        }
    }
}
