using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Web;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;
using static TDS.Server.Data.Interfaces.PlayersSystem.IPlayerEvents;

namespace TDS.Server.Handler.PlayerHandlers
{
    public class PlayerSettingsSyncHandler
    {
        public PlayerSettingsSyncHandler(EventsHandler eventsHandler)
        {
            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
        }

        private void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            SyncPlayerSettings(player);
        }

        public object? RequestSyncPlayerSettingsFromUserpanel(ITDSPlayer player, ref ArraySegment<object> arg)
        {
            SyncPlayerSettings(player);
            return null;
        }

        private void SyncPlayerSettings(ITDSPlayer player)
        {
            var model = new SyncedPlayerSettings
            {
                Client = GetForClient(player),
                AngularJson = Serializer.ToBrowser(GetForAngular(player))
            };
            var json = Serializer.ToClient(model);
            NAPI.Task.Run(() => 
                player.TriggerEvent(ToClientEvent.SyncSettings, json));
        }

        private SyncedClientPlayerSettings GetForClient(ITDSPlayer player)
        {
            if (player.Entity?.PlayerSettings is null)
                return new SyncedClientPlayerSettings();

            var s = player.Entity.PlayerSettings;
            return new SyncedClientPlayerSettings
            {
                AFKKickAfterSeconds = s.CooldownsAndDurations.AFKKickAfterSeconds,
                AFKKickShowWarningLastSeconds = s.CooldownsAndDurations.AFKKickShowWarningLastSeconds,
                Bloodscreen = s.FightEffect.Bloodscreen,
                BloodscreenCooldownMs = s.CooldownsAndDurations.BloodscreenCooldownMs,
                CheckAFK = s.General.CheckAFK,
                FloatingDamageInfo = s.FightEffect.FloatingDamageInfo,
                Hitsound = s.FightEffect.Hitsound,
                HudAmmoUpdateCooldownMs = s.CooldownsAndDurations.HudAmmoUpdateCooldownMs,
                HudHealthUpdateCooldownMs = s.CooldownsAndDurations.HudHealthUpdateCooldownMs,
                Language = s.General.Language,
                MapBorderColor = s.IngameColors.MapBorderColor,
                NametagArmorEmptyColor = s.IngameColors.NametagArmorEmptyColor,
                NametagArmorFullColor = s.IngameColors.NametagArmorFullColor,
                NametagDeadColor = s.IngameColors.NametagDeadColor,
                NametagHealthEmptyColor = s.IngameColors.NametagHealthEmptyColor,
                NametagHealthFullColor = s.IngameColors.NametagHealthFullColor,
                ScoreboardPlayerSorting = s.Scoreboard.ScoreboardPlayerSorting,
                ScoreboardPlayerSortingDesc = s.Scoreboard.ScoreboardPlayerSortingDesc,
                ScoreboardPlaytimeUnit = s.Scoreboard.ScoreboardPlaytimeUnit,
                ShowConfettiAtRanking = s.General.ShowConfettiAtRanking,
                ShowCursorInfo = s.Info.ShowCursorInfo,
                ShowCursorOnChatOpen = s.Chat.ShowCursorOnChatOpen,
                ShowFloatingDamageInfoDurationMs = s.CooldownsAndDurations.ShowFloatingDamageInfoDurationMs,
                ShowLobbyLeaveInfo = s.Info.ShowLobbyLeaveInfo,
                Voice3D = s.Voice.Voice3D,
                VoiceAutoVolume = s.Voice.VoiceAutoVolume,
                VoiceVolume = s.Voice.VoiceVolume,
                WindowsNotifications = s.General.WindowsNotifications
            };
        }

        private SyncedAngularPlayerSettings GetForAngular(ITDSPlayer player)
        {
            if (player.Entity?.PlayerSettings is null)
                return new SyncedAngularPlayerSettings();

            var s = player.Entity.PlayerSettings;
            return new SyncedAngularPlayerSettings
            {
                ChatWidth = s.Chat.ChatWidth,
                ChatMaxHeight = s.Chat.ChatMaxHeight,
                ChatFontSize = s.Chat.ChatFontSize,
                HideDirtyChat = s.Chat.HideDirtyChat,
                HideChatInfo = s.Chat.HideChatInfo,
                ChatInfoFontSize = s.Chat.ChatInfoFontSize,
                ChatInfoMoveTimeMs = s.Chat.ChatInfoMoveTimeMs,

                KillInfoShowIcon = player.Entity.KillInfoSettings.ShowIcon,
                KillInfoFontSize = player.Entity.KillInfoSettings.FontSize,
                KillInfoIconWidth = player.Entity.KillInfoSettings.IconWidth,
                KillInfoSpacing = player.Entity.KillInfoSettings.Spacing,
                KillInfoDuration = player.Entity.KillInfoSettings.Duration,
                KillInfoIconHeight = player.Entity.KillInfoSettings.IconHeight,

                UseDarkTheme = player.Entity.ThemeSettings.UseDarkTheme,
                ThemeMainColor = player.Entity.ThemeSettings.ThemeMainColor,
                ThemeSecondaryColor = player.Entity.ThemeSettings.ThemeSecondaryColor,
                ThemeWarnColor = player.Entity.ThemeSettings.ThemeWarnColor,
                ThemeBackgroundDarkColor = player.Entity.ThemeSettings.ThemeBackgroundDarkColor,
                ThemeBackgroundLightColor = player.Entity.ThemeSettings.ThemeBackgroundLightColor,
                ToolbarDesign = player.Entity.ThemeSettings.ToolbarDesign
            };
        }
    }
}
