using AltV.Net;

namespace TDS_Shared.Data.Models.Settings
{
    public class PlayerOtherColorsSettings : IMValueConvertible
    {
        public string MapBorderColor { get; set; }
        public string NametagArmorEmptyColor { get; set; }
        public string NametagArmorFullColor { get; set; }
        public string NametagDeadColor { get; set; }
        public string NametagHealthEmptyColor { get; set; }
        public string NametagHealthFullColor { get; set; }


        private readonly IMValueBaseAdapter _adapter = new PlayerOtherColorsSettingsAdapter();

        public IMValueBaseAdapter GetAdapter() => _adapter;

        public class PlayerOtherColorsSettingsAdapter : IMValueAdapter<PlayerOtherColorsSettings>
        {
            public PlayerOtherColorsSettings FromMValue(IMValueReader reader)
            {
                var obj = new PlayerOtherColorsSettings();

                reader.BeginObject();

                while (reader.HasNext())
                {
                    switch (reader.NextName())
                    {
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
                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(PlayerOtherColorsSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

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

                writer.EndObject();
            }

            public void ToMValue(object obj, IMValueWriter writer)
            {
                if (obj is PlayerOtherColorsSettings value)
                    ToMValue(value, writer);
            }

            object IMValueBaseAdapter.FromMValue(IMValueReader reader)
                => FromMValue(reader);
        }
    }
}
