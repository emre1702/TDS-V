using AltV.Net;

namespace TDS_Shared.Data.Models.Settings
{
    public class PlayerFightSettings : IMValueConvertible
    {
        public bool Bloodscreen { get; set; }
        public bool Hitsound { get; set; }
        public bool FloatingDamageInfo { get; set; }


        private readonly IMValueBaseAdapter _adapter = new PlayerFightSettingsAdapter();

        public IMValueBaseAdapter GetAdapter() => _adapter;

        public class PlayerFightSettingsAdapter : IMValueAdapter<PlayerFightSettings>
        {
            public PlayerFightSettings FromMValue(IMValueReader reader)
            {
                var obj = new PlayerFightSettings();

                reader.BeginObject();

                while (reader.HasNext())
                {
                    switch (reader.NextName())
                    {
                        case nameof(Bloodscreen):
                            obj.Bloodscreen = reader.NextBool();
                            break;
                        case nameof(Hitsound):
                            obj.Hitsound = reader.NextBool();
                            break;
                        case nameof(FloatingDamageInfo):
                            obj.FloatingDamageInfo = reader.NextBool();
                            break;
                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(PlayerFightSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

                writer.Name(nameof(Bloodscreen));
                writer.Value(value.Bloodscreen);

                writer.Name(nameof(Hitsound));
                writer.Value(value.Hitsound);

                writer.Name(nameof(FloatingDamageInfo));
                writer.Value(value.FloatingDamageInfo);

                writer.EndObject();
            }

            public void ToMValue(object obj, IMValueWriter writer)
            {
                if (obj is PlayerFightSettings value)
                    ToMValue(value, writer);
            }

            object IMValueBaseAdapter.FromMValue(IMValueReader reader)
                => FromMValue(reader);
        }
    }
}
