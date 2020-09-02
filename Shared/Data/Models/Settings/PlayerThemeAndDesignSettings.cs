using AltV.Net;

namespace TDS_Shared.Data.Models.Settings
{
    public class PlayerThemeAndDesignSettings : IMValueConvertible
    {
        public string ThemeBackgroundDarkColor { get; set; }
        public string ThemeBackgroundLightColor { get; set; }
        public string ThemeMainColor { get; set; }
        public string ThemeSecondaryColor { get; set; }
        public string ThemeWarnColor { get; set; }
        public int ToolbarDesign { get; set; }
        public bool UseDarkTheme { get; set; }


        private readonly IMValueBaseAdapter _adapter = new PlayerThemeAndDesignSettingsAdapter();

        public IMValueBaseAdapter GetAdapter() => _adapter;

        public class PlayerThemeAndDesignSettingsAdapter : IMValueAdapter<PlayerThemeAndDesignSettings>
        {
            public PlayerThemeAndDesignSettings FromMValue(IMValueReader reader)
            {
                var obj = new PlayerThemeAndDesignSettings();

                reader.BeginObject();

                while (reader.HasNext())
                {
                    switch (reader.NextName())
                    {
                        case nameof(ThemeBackgroundDarkColor):
                            obj.ThemeBackgroundDarkColor = reader.NextString();
                            break;
                        case nameof(ThemeBackgroundLightColor):
                            obj.ThemeBackgroundLightColor = reader.NextString();
                            break;
                        case nameof(ThemeMainColor):
                            obj.ThemeMainColor = reader.NextString();
                            break;
                        case nameof(ThemeSecondaryColor):
                            obj.ThemeSecondaryColor = reader.NextString();
                            break;
                        case nameof(ThemeWarnColor):
                            obj.ThemeWarnColor = reader.NextString();
                            break;
                        case nameof(ToolbarDesign):
                            obj.ToolbarDesign = reader.NextInt();
                            break;
                        case nameof(UseDarkTheme):
                            obj.UseDarkTheme = reader.NextBool();
                            break;

                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(PlayerThemeAndDesignSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

                writer.Name(nameof(ThemeBackgroundDarkColor));
                writer.Value(value.ThemeBackgroundDarkColor);

                writer.Name(nameof(ThemeBackgroundLightColor));
                writer.Value(value.ThemeBackgroundLightColor);

                writer.Name(nameof(ThemeMainColor));
                writer.Value(value.ThemeMainColor);

                writer.Name(nameof(ThemeSecondaryColor));
                writer.Value(value.ThemeSecondaryColor);

                writer.Name(nameof(ThemeWarnColor));
                writer.Value(value.ThemeWarnColor);

                writer.Name(nameof(ToolbarDesign));
                writer.Value(value.ToolbarDesign);

                writer.Name(nameof(UseDarkTheme));
                writer.Value(value.UseDarkTheme);

                writer.EndObject();
            }

            public void ToMValue(object obj, IMValueWriter writer)
            {
                if (obj is PlayerThemeAndDesignSettings value)
                    ToMValue(value, writer);
            }

            object IMValueBaseAdapter.FromMValue(IMValueReader reader)
                => FromMValue(reader);
        }
    }
}
