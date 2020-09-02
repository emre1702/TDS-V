using AltV.Net;
using TDS_Shared.Data.Enums;

namespace TDS_Shared.Data.Models.Settings
{
    public class PlayerScoreboardSettings : IMValueConvertible
    {
        public ScoreboardPlayerSorting ScoreboardPlayerSorting { get; set; }
        public bool ScoreboardPlayerSortingDesc { get; set; }
        public TimeSpanUnitsOfTime ScoreboardPlaytimeUnit { get; set; }


        private readonly IMValueBaseAdapter _adapter = new PlayerScoreboardSettingsAdapter();

        public IMValueBaseAdapter GetAdapter() => _adapter;

        public class PlayerScoreboardSettingsAdapter : IMValueAdapter<PlayerScoreboardSettings>
        {
            public PlayerScoreboardSettings FromMValue(IMValueReader reader)
            {
                var obj = new PlayerScoreboardSettings();

                reader.BeginObject();

                while (reader.HasNext())
                {
                    switch (reader.NextName())
                    {
                        case nameof(ScoreboardPlayerSorting):
                            obj.ScoreboardPlayerSorting = (ScoreboardPlayerSorting)reader.NextInt();
                            break;
                        case nameof(ScoreboardPlayerSortingDesc):
                            obj.ScoreboardPlayerSortingDesc = reader.NextBool();
                            break;
                        case nameof(ScoreboardPlaytimeUnit):
                            obj.ScoreboardPlaytimeUnit = (TimeSpanUnitsOfTime)reader.NextInt();
                            break;
                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(PlayerScoreboardSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

                writer.Name(nameof(ScoreboardPlayerSorting));
                writer.Value((int)value.ScoreboardPlayerSorting);

                writer.Name(nameof(ScoreboardPlayerSortingDesc));
                writer.Value(value.ScoreboardPlayerSortingDesc);

                writer.Name(nameof(ScoreboardPlaytimeUnit));
                writer.Value((int)value.ScoreboardPlaytimeUnit);

                writer.EndObject();
            }

            public void ToMValue(object obj, IMValueWriter writer)
            {
                if (obj is PlayerScoreboardSettings value)
                    ToMValue(value, writer);
            }

            object IMValueBaseAdapter.FromMValue(IMValueReader reader)
                => FromMValue(reader);
        }
    }
}
