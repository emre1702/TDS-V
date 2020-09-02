using AltV.Net;

namespace TDS_Shared.Data.Models.Settings
{
    public class PlayerTimesSettings : IMValueConvertible
    {
        public int BloodscreenCooldownMs { get; set; }
        public int HudAmmoUpdateCooldownMs { get; set; }
        public int HudHealthUpdateCooldownMs { get; set; }
        public int AFKKickAfterSeconds { get; set; }
        public int AFKKickShowWarningLastSeconds { get; set; }
        public int ShowFloatingDamageInfoDurationMs { get; set; }


        private readonly IMValueBaseAdapter _adapter = new PlayerTimesSettingsAdapter();

        public IMValueBaseAdapter GetAdapter() => _adapter;

        public class PlayerTimesSettingsAdapter : IMValueAdapter<PlayerTimesSettings>
        {
            public PlayerTimesSettings FromMValue(IMValueReader reader)
            {
                var obj = new PlayerTimesSettings();

                reader.BeginObject();

                while (reader.HasNext())
                {
                    switch (reader.NextName())
                    {
                        case nameof(BloodscreenCooldownMs):
                            obj.BloodscreenCooldownMs = reader.NextInt();
                            break;
                        case nameof(HudAmmoUpdateCooldownMs):
                            obj.HudAmmoUpdateCooldownMs = reader.NextInt();
                            break;
                        case nameof(HudHealthUpdateCooldownMs):
                            obj.HudHealthUpdateCooldownMs = reader.NextInt();
                            break;
                        case nameof(AFKKickAfterSeconds):
                            obj.AFKKickAfterSeconds = reader.NextInt();
                            break;
                        case nameof(AFKKickShowWarningLastSeconds):
                            obj.AFKKickShowWarningLastSeconds = reader.NextInt();
                            break;
                        case nameof(ShowFloatingDamageInfoDurationMs):
                            obj.ShowFloatingDamageInfoDurationMs = reader.NextInt();
                            break;
                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(PlayerTimesSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

                writer.Name(nameof(BloodscreenCooldownMs));
                writer.Value(value.BloodscreenCooldownMs);

                writer.Name(nameof(HudAmmoUpdateCooldownMs));
                writer.Value(value.HudAmmoUpdateCooldownMs);

                writer.Name(nameof(HudHealthUpdateCooldownMs));
                writer.Value(value.HudHealthUpdateCooldownMs);

                writer.Name(nameof(AFKKickAfterSeconds));
                writer.Value(value.AFKKickAfterSeconds);

                writer.Name(nameof(AFKKickShowWarningLastSeconds));
                writer.Value(value.AFKKickShowWarningLastSeconds);

                writer.Name(nameof(ShowFloatingDamageInfoDurationMs));
                writer.Value(value.ShowFloatingDamageInfoDurationMs);

                writer.EndObject();
            }

            public void ToMValue(object obj, IMValueWriter writer)
            {
                if (obj is PlayerTimesSettings value)
                    ToMValue(value, writer);
            }

            object IMValueBaseAdapter.FromMValue(IMValueReader reader)
                => FromMValue(reader);
        }
    }
}
