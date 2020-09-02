using AltV.Net;
using TDS_Shared.Data.Extensions;

namespace TDS_Shared.Data.Models.Settings
{
    public class PlayerVoiceSettings : IMValueConvertible
    {
        public bool Voice3D { get; set; }
        public bool VoiceAutoVolume { get; set; }
        public float VoiceVolume { get; set; }


        private readonly IMValueBaseAdapter _adapter = new PlayerVoiceSettingsAdapter();

        public IMValueBaseAdapter GetAdapter() => _adapter;

        public class PlayerVoiceSettingsAdapter : IMValueAdapter<PlayerVoiceSettings>
        {
            public PlayerVoiceSettings FromMValue(IMValueReader reader)
            {
                var obj = new PlayerVoiceSettings();

                reader.BeginObject();

                while (reader.HasNext())
                {
                    switch (reader.NextName())
                    {
                        case nameof(Voice3D):
                            obj.Voice3D = reader.NextBool();
                            break;
                        case nameof(VoiceAutoVolume):
                            obj.VoiceAutoVolume = reader.NextBool();
                            break;
                        case nameof(VoiceVolume):
                            obj.VoiceVolume = reader.NextFloat();
                            break;
                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(PlayerVoiceSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

                writer.Name(nameof(Voice3D));
                writer.Value(value.Voice3D);

                writer.Name(nameof(VoiceAutoVolume));
                writer.Value(value.VoiceAutoVolume);

                writer.Name(nameof(VoiceVolume));
                writer.Value(value.VoiceVolume);

                writer.EndObject();
            }

            public void ToMValue(object obj, IMValueWriter writer)
            {
                if (obj is PlayerVoiceSettings value)
                    ToMValue(value, writer);
            }

            object IMValueBaseAdapter.FromMValue(IMValueReader reader)
                => FromMValue(reader);
        }
    }
}
