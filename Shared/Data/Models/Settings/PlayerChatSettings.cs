using AltV.Net;
using TDS_Shared.Data.Extensions;

namespace TDS_Shared.Data.Models.Settings
{
    public class PlayerChatSettings : IMValueConvertible
    {
        public float FontSize { get; set; }
        public bool HideInfo { get; set; }
        public bool HideDirty { get; set; }
        public float InfoFontSize { get; set; }
        public int InfoMoveTimeMs { get; set; }
        public float MaxHeight { get; set; }
        public float Width { get; set; }
        public bool ShowCursorOnChatOpen { get; set; }


        private readonly IMValueBaseAdapter _adapter = new PlayerChatSettingsAdapter();


        public IMValueBaseAdapter GetAdapter()
            => _adapter;


        private class PlayerChatSettingsAdapter : IMValueAdapter<PlayerChatSettings>
        {
            public PlayerChatSettings FromMValue(IMValueReader reader)
            {
                var obj = new PlayerChatSettings();

                reader.BeginObject();

                while (reader.HasNext())
                {
                    switch (reader.NextName())
                    {

                        case nameof(FontSize):
                            obj.FontSize = reader.NextFloat();
                            break;
                        case nameof(InfoFontSize):
                            obj.InfoFontSize = reader.NextFloat();
                            break;
                        case nameof(InfoMoveTimeMs):
                            obj.InfoMoveTimeMs = reader.NextInt();
                            break;
                        case nameof(MaxHeight):
                            obj.MaxHeight = reader.NextFloat();
                            break;
                        case nameof(Width):
                            obj.Width = reader.NextFloat();
                            break;
                        case nameof(HideInfo):
                            obj.HideInfo = reader.NextBool();
                            break;
                        case nameof(HideDirty):
                            obj.HideDirty = reader.NextBool();
                            break;
                        case nameof(ShowCursorOnChatOpen):
                            obj.ShowCursorOnChatOpen = reader.NextBool();
                            break;
                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(PlayerChatSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

                writer.Name(nameof(FontSize));
                writer.Value(value.FontSize);

                writer.Name(nameof(InfoFontSize));
                writer.Value(value.InfoFontSize);

                writer.Name(nameof(InfoMoveTimeMs));
                writer.Value(value.InfoMoveTimeMs);

                writer.Name(nameof(MaxHeight));
                writer.Value(value.MaxHeight);

                writer.Name(nameof(Width));
                writer.Value(value.Width);

                writer.Name(nameof(HideInfo));
                writer.Value(value.HideInfo);

                writer.Name(nameof(HideDirty));
                writer.Value(value.HideDirty);

                writer.Name(nameof(ShowCursorOnChatOpen));
                writer.Value(value.ShowCursorOnChatOpen);

                writer.EndObject();
            }

            public void ToMValue(object obj, IMValueWriter writer)
            {
                if (obj is PlayerChatSettings value)
                    ToMValue(value, writer);
            }

            object IMValueBaseAdapter.FromMValue(IMValueReader reader)
                => FromMValue(reader);
        }
    }

}
