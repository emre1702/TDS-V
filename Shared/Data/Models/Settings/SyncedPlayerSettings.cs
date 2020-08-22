using AltV.Net;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Extensions;

namespace TDS_Shared.Data.Models.Settings
{
    public class SyncedPlayerSettings : IMValueConvertible
    {
        #region Properties

        public int AFKKickAfterSeconds { get; set; }
        public int AFKKickShowWarningLastSeconds { get; set; }

        public bool AllowDataTransfer { get; set; }

        public bool Bloodscreen { get; set; }

        public int BloodscreenCooldownMs { get; set; }

        public bool CheckAFK { get; set; }

        public string DateTimeFormat { get; set; }

        public ulong? DiscordUserId { get; set; }
        public bool FloatingDamageInfo { get; set; }


        public bool Hitsound { get; set; }
        public int HudAmmoUpdateCooldownMs { get; set; }

        public int HudHealthUpdateCooldownMs { get; set; }

        public Language Language { get; set; }
        public string MapBorderColor { get; set; }

        public string NametagArmorEmptyColor { get; set; }

        public string NametagArmorFullColor { get; set; }
        public string NametagDeadColor { get; set; }
        public string NametagHealthEmptyColor { get; set; }
        public string NametagHealthFullColor { get; set; }

        public int PlayerId { get; set; }

        public ScoreboardPlayerSorting ScoreboardPlayerSorting { get; set; }
        public bool ScoreboardPlayerSortingDesc { get; set; }

        public TimeSpanUnitsOfTime ScoreboardPlaytimeUnit { get; set; }

        public bool ShowConfettiAtRanking { get; set; }

        public bool ShowCursorInfo { get; set; }

        public bool ShowCursorOnChatOpen { get; set; }

        public int ShowFloatingDamageInfoDurationMs { get; set; }

        public bool ShowLobbyLeaveInfo { get; set; }

        public string Timezone { get; set; }

        public bool Voice3D { get; set; }

        public bool VoiceAutoVolume { get; set; }

        public float VoiceVolume { get; set; }

        public bool WindowsNotifications { get; set; }

        public SyncedPlayerAngularChatSettings Chat { get; set; }

        private static readonly IMValueBaseAdapter _adapter = new SyncedPlayerSettingsAdapter();

        public IMValueBaseAdapter GetAdapter()
            => _adapter;

        private class SyncedPlayerSettingsAdapter : IMValueAdapter<SyncedPlayerSettings>
        {
            public SyncedPlayerSettings FromMValue(IMValueReader reader)
            {
                var obj = new SyncedPlayerSettings();

                reader.BeginObject();

                while (reader.HasNext())
                {
                    switch (reader.NextName())
                    {
                        case nameof(AFKKickAfterSeconds):
                            obj.AFKKickAfterSeconds = reader.NextInt();
                            break;
                        case nameof(AFKKickShowWarningLastSeconds):
                            obj.AFKKickShowWarningLastSeconds = reader.NextInt();
                            break;
                        case nameof(AllowDataTransfer):
                            obj.AllowDataTransfer = reader.NextBool();
                            break;
                        case nameof(Bloodscreen):
                            obj.Bloodscreen = reader.NextBool();
                            break;
                        case nameof(BloodscreenCooldownMs):
                            obj.BloodscreenCooldownMs = reader.NextInt();
                            break;
                        case nameof(CheckAFK):
                            obj.CheckAFK = reader.NextBool();
                            break;
                        case nameof(DateTimeFormat):
                            obj.DateTimeFormat = reader.NextString();
                            break;
                        case nameof(DiscordUserId):
                            obj.DiscordUserId = reader.NextULong();
                            break;
                        case nameof(FloatingDamageInfo):
                            obj.FloatingDamageInfo = reader.NextBool();
                            break;
                        case nameof(Hitsound):
                            obj.Hitsound = reader.NextBool();
                            break;
                        case nameof(HudAmmoUpdateCooldownMs):
                            obj.HudAmmoUpdateCooldownMs = reader.NextInt();
                            break;
                        case nameof(HudHealthUpdateCooldownMs):
                            obj.HudHealthUpdateCooldownMs = reader.NextInt();
                            break;
                        case nameof(Language):
                            obj.Language = (Language)reader.NextInt();
                            break;
                        case nameof(MapBorderColor):
                            obj.MapBorderColor = reader.NextString();
                            break;
                        case nameof(NametagArmorEmptyColor):
                            obj.NametagArmorEmptyColor = reader.NextString();
                            break;
                        case nameof(NametagArmorFullColor):
                            obj.NametagArmorFullColor = reader.NextString();
                            break;
                        case nameof(NametagDeadColor):
                            obj.NametagDeadColor = reader.NextString();
                            break;
                        case nameof(NametagHealthEmptyColor):
                            obj.NametagHealthEmptyColor = reader.NextString();
                            break;
                        case nameof(NametagHealthFullColor):
                            obj.NametagHealthFullColor = reader.NextString();
                            break;
                        case nameof(PlayerId):
                            obj.PlayerId = reader.NextInt();
                            break;
                        case nameof(ScoreboardPlayerSorting):
                            obj.ScoreboardPlayerSorting = (ScoreboardPlayerSorting)reader.NextInt();
                            break;
                        case nameof(ScoreboardPlayerSortingDesc):
                            obj.ScoreboardPlayerSortingDesc = reader.NextBool();
                            break;
                        case nameof(ScoreboardPlaytimeUnit):
                            obj.ScoreboardPlaytimeUnit = (TimeSpanUnitsOfTime)reader.NextInt();
                            break;
                        case nameof(ShowConfettiAtRanking):
                            obj.ShowConfettiAtRanking = reader.NextBool();
                            break;
                        case nameof(ShowCursorInfo):
                            obj.ShowCursorInfo = reader.NextBool();
                            break;
                        case nameof(ShowCursorOnChatOpen):
                            obj.ShowCursorOnChatOpen = reader.NextBool();
                            break;
                        case nameof(ShowFloatingDamageInfoDurationMs):
                            obj.ShowFloatingDamageInfoDurationMs = reader.NextInt();
                            break;
                        case nameof(ShowLobbyLeaveInfo):
                            obj.ShowLobbyLeaveInfo = reader.NextBool();
                            break;
                        case nameof(Timezone):
                            obj.Timezone = reader.NextString();
                            break;
                        case nameof(Voice3D):
                            obj.Voice3D = reader.NextBool();
                            break;
                        case nameof(VoiceAutoVolume):
                            obj.VoiceAutoVolume = reader.NextBool();
                            break;
                        case nameof(VoiceVolume):
                            obj.VoiceVolume = reader.NextFloat();
                            break;
                        case nameof(WindowsNotifications):
                            obj.WindowsNotifications = reader.NextBool();
                            break;


                        case nameof(Chat):
                            obj.Chat = (SyncedPlayerAngularChatSettings)new SyncedPlayerAngularChatSettings().GetAdapter().FromMValue(reader);
                            break;
                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(SyncedPlayerSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

                writer.Name(nameof(AFKKickAfterSeconds));
                writer.Value(value.AFKKickAfterSeconds);

                writer.Name(nameof(AFKKickShowWarningLastSeconds));
                writer.Value(value.AFKKickShowWarningLastSeconds);

                writer.Name(nameof(AllowDataTransfer));
                writer.Value(value.AllowDataTransfer);

                writer.Name(nameof(Bloodscreen));
                writer.Value(value.Bloodscreen);

                writer.Name(nameof(BloodscreenCooldownMs));
                writer.Value(value.BloodscreenCooldownMs);

                writer.Name(nameof(CheckAFK));
                writer.Value(value.CheckAFK);

                writer.Name(nameof(DateTimeFormat));
                writer.Value(value.DateTimeFormat);

                if (value.DiscordUserId.HasValue)
                {
                    writer.Name(nameof(DiscordUserId));
                    writer.Value(value.DiscordUserId.Value);
                }

                writer.Name(nameof(FloatingDamageInfo));
                writer.Value(value.FloatingDamageInfo);

                writer.Name(nameof(Hitsound));
                writer.Value(value.Hitsound);

                writer.Name(nameof(HudAmmoUpdateCooldownMs));
                writer.Value(value.HudAmmoUpdateCooldownMs);

                writer.Name(nameof(HudHealthUpdateCooldownMs));
                writer.Value(value.HudHealthUpdateCooldownMs);

                writer.Name(nameof(Language));
                writer.Value((int)value.Language);

                writer.Name(nameof(MapBorderColor));
                writer.Value(value.MapBorderColor);

                writer.Name(nameof(NametagArmorEmptyColor));
                writer.Value(value.NametagArmorEmptyColor);

                writer.Name(nameof(NametagArmorFullColor));
                writer.Value(value.NametagArmorFullColor);

                writer.Name(nameof(NametagDeadColor));
                writer.Value(value.NametagDeadColor);

                writer.Name(nameof(NametagHealthEmptyColor));
                writer.Value(value.NametagHealthEmptyColor);

                writer.Name(nameof(NametagHealthFullColor));
                writer.Value(value.NametagHealthFullColor);

                writer.Name(nameof(PlayerId));
                writer.Value(value.PlayerId);

                writer.Name(nameof(ScoreboardPlayerSorting));
                writer.Value((int)value.ScoreboardPlayerSorting);

                writer.Name(nameof(ScoreboardPlayerSortingDesc));
                writer.Value(value.ScoreboardPlayerSortingDesc);

                writer.Name(nameof(ScoreboardPlaytimeUnit));
                writer.Value((int)value.ScoreboardPlaytimeUnit);

                writer.Name(nameof(ShowConfettiAtRanking));
                writer.Value(value.ShowConfettiAtRanking);

                writer.Name(nameof(ShowCursorInfo));
                writer.Value(value.ShowCursorInfo);

                writer.Name(nameof(ShowCursorOnChatOpen));
                writer.Value(value.ShowCursorOnChatOpen);

                writer.Name(nameof(ShowFloatingDamageInfoDurationMs));
                writer.Value(value.ShowFloatingDamageInfoDurationMs);

                writer.Name(nameof(ShowLobbyLeaveInfo));
                writer.Value(value.ShowLobbyLeaveInfo);

                writer.Name(nameof(Timezone));
                writer.Value(value.Timezone);

                writer.Name(nameof(Voice3D));
                writer.Value(value.Voice3D);

                writer.Name(nameof(VoiceAutoVolume));
                writer.Value(value.VoiceAutoVolume);

                writer.Name(nameof(VoiceVolume));
                writer.Value(value.VoiceVolume);

                writer.Name(nameof(WindowsNotifications));
                writer.Value(value.WindowsNotifications);

                writer.Name(nameof(Chat));
                value.Chat.GetAdapter().ToMValue(value.Chat, writer);

                writer.EndObject();
            }

            public void ToMValue(object obj, IMValueWriter writer)
            {
                if (obj is SyncedPlayerSettings value)
                    ToMValue(value, writer);
            }

            object IMValueBaseAdapter.FromMValue(IMValueReader reader)
                => FromMValue(reader);
        }

        #endregion Properties
    }
}
