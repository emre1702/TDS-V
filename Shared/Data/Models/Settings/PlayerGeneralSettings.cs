using AltV.Net;
using TDS_Shared.Data.Enums;

namespace TDS_Shared.Data.Models.Settings
{
    public class PlayerGeneralSettings : IMValueConvertible
    {
        public Language Language { get; set; }
        public bool AllowDataTransfer { get; set; }
        public bool ShowConfettiAtRanking { get; set; }
        public string Timezone { get; set; }
        public string DateTimeFormat { get; set; }
        public ulong? DiscordUserId { get; set; }
        public bool CheckAFK { get; set; }
        public bool WindowsNotifications { get; set; }


        private readonly PlayerGeneralSettingsAdapter _adapter = new PlayerGeneralSettingsAdapter();

        public IMValueBaseAdapter GetAdapter() => _adapter;

        private class PlayerGeneralSettingsAdapter : IMValueAdapter<PlayerGeneralSettings>
        {
            public PlayerGeneralSettings FromMValue(IMValueReader reader)
            {
                var obj = new PlayerGeneralSettings();

                reader.BeginObject();

                while (reader.HasNext())
                {
                    switch (reader.NextName())
                    {

                        case nameof(Language):
                            obj.Language = (Language)reader.NextInt();
                            break;
                        case nameof(AllowDataTransfer):
                            obj.AllowDataTransfer = reader.NextBool();
                            break;
                        case nameof(ShowConfettiAtRanking):
                            obj.ShowConfettiAtRanking = reader.NextBool();
                            break;
                        case nameof(Timezone):
                            obj.Timezone = reader.NextString();
                            break;
                        case nameof(DateTimeFormat):
                            obj.DateTimeFormat = reader.NextString();
                            break;
                        case nameof(DiscordUserId):
                            obj.DiscordUserId = reader.NextULong();
                            break;
                        case nameof(CheckAFK):
                            obj.CheckAFK = reader.NextBool();
                            break;
                        case nameof(WindowsNotifications):
                            obj.WindowsNotifications = reader.NextBool();
                            break;
                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(PlayerGeneralSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

                writer.Name(nameof(Language));
                writer.Value((int)value.Language);

                writer.Name(nameof(AllowDataTransfer));
                writer.Value(value.AllowDataTransfer);

                writer.Name(nameof(ShowConfettiAtRanking));
                writer.Value(value.ShowConfettiAtRanking);

                writer.Name(nameof(Timezone));
                writer.Value(value.Timezone);

                writer.Name(nameof(DateTimeFormat));
                writer.Value(value.DateTimeFormat);

                if (value.DiscordUserId.HasValue)
                {
                    writer.Name(nameof(DiscordUserId));
                    writer.Value(value.DiscordUserId.Value);
                }

                writer.Name(nameof(CheckAFK));
                writer.Value(value.CheckAFK);

                writer.Name(nameof(WindowsNotifications));
                writer.Value(value.WindowsNotifications);

                writer.EndObject();
            }

            public void ToMValue(object obj, IMValueWriter writer)
            {
                if (obj is PlayerGeneralSettings value)
                    ToMValue(value, writer);
            }

            object IMValueBaseAdapter.FromMValue(IMValueReader reader)
                => FromMValue(reader);
        }
    }
}
