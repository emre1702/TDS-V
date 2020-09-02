using AltV.Net;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Extensions;

namespace TDS_Shared.Data.Models.Settings
{
    public class SyncedPlayerSettings : IMValueConvertible
    {
        #region Properties
        public int PlayerId { get; set; }

        public PlayerChatSettings Chat { get; set; }
        public PlayerGeneralSettings General { get; set; }
        public PlayerFightSettings Fight { get; set; }
        public PlayerThemeAndDesignSettings ThemeAndDesign { get; set; }
        public PlayerVoiceSettings Voice { get; set; }
        public PlayerOtherColorsSettings OtherColors { get; set; }
        public PlayerTimesSettings Times { get; set; }
        public PlayerScoreboardSettings Scoreboard { get; set; }
        public PlayerInfoSettings Info { get; set; }

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
                        case nameof(PlayerId):
                            obj.PlayerId = reader.NextInt();
                            break;
         
                        case nameof(Chat):
                            obj.Chat = (PlayerChatSettings)new PlayerChatSettings().GetAdapter().FromMValue(reader);
                            break;
                        case nameof(General):
                            obj.General = (PlayerGeneralSettings)new PlayerGeneralSettings().GetAdapter().FromMValue(reader);
                            break;
                        case nameof(Fight):
                            obj.Fight = (PlayerFightSettings)new PlayerFightSettings().GetAdapter().FromMValue(reader);
                            break;
                        case nameof(ThemeAndDesign):
                            obj.ThemeAndDesign = (PlayerThemeAndDesignSettings)new PlayerThemeAndDesignSettings().GetAdapter().FromMValue(reader);
                            break;
                        case nameof(Voice):
                            obj.Voice = (PlayerVoiceSettings)new PlayerVoiceSettings().GetAdapter().FromMValue(reader);
                            break;
                        case nameof(OtherColors):
                            obj.OtherColors = (PlayerOtherColorsSettings)new PlayerOtherColorsSettings().GetAdapter().FromMValue(reader);
                            break;
                        case nameof(Times):
                            obj.Times = (PlayerTimesSettings)new PlayerTimesSettings().GetAdapter().FromMValue(reader);
                            break;
                        case nameof(Scoreboard):
                            obj.Scoreboard = (PlayerScoreboardSettings)new PlayerScoreboardSettings().GetAdapter().FromMValue(reader);
                            break;
                        case nameof(Info):
                            obj.Info = (PlayerInfoSettings)new PlayerInfoSettings().GetAdapter().FromMValue(reader);
                            break;
                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(SyncedPlayerSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

                
                writer.Name(nameof(PlayerId));
                writer.Value(value.PlayerId);


                writer.Name(nameof(Chat));
                value.Chat.GetAdapter().ToMValue(value.Chat, writer);

                writer.Name(nameof(General));
                value.General.GetAdapter().ToMValue(value.General, writer);

                writer.Name(nameof(ThemeAndDesign));
                value.ThemeAndDesign.GetAdapter().ToMValue(value.ThemeAndDesign, writer);

                writer.Name(nameof(Voice));
                value.Voice.GetAdapter().ToMValue(value.Voice, writer);

                writer.Name(nameof(OtherColors));
                value.OtherColors.GetAdapter().ToMValue(value.OtherColors, writer);

                writer.Name(nameof(Times));
                value.Times.GetAdapter().ToMValue(value.Times, writer);

                writer.Name(nameof(Scoreboard));
                value.Scoreboard.GetAdapter().ToMValue(value.Scoreboard, writer);

                writer.Name(nameof(Info));
                value.Info.GetAdapter().ToMValue(value.Info, writer);

                writer.Name(nameof(Fight));
                value.Fight.GetAdapter().ToMValue(value.Fight, writer);

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
