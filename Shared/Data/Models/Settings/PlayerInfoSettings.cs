using AltV.Net;

namespace TDS_Shared.Data.Models.Settings
{
    public class PlayerInfoSettings : IMValueConvertible
    {
        public bool ShowCursorInfo { get; set; }
        public bool ShowLobbyLeaveInfo { get; set; }


        private readonly IMValueBaseAdapter _adapter = new PlayerInfoSettingsAdapter();

        public IMValueBaseAdapter GetAdapter() => _adapter;

        public class PlayerInfoSettingsAdapter : IMValueAdapter<PlayerInfoSettings>
        {
            public PlayerInfoSettings FromMValue(IMValueReader reader)
            {
                var obj = new PlayerInfoSettings();

                reader.BeginObject();

                while (reader.HasNext())
                {
                    switch (reader.NextName())
                    {
                        case nameof(ShowCursorInfo):
                            obj.ShowCursorInfo = reader.NextBool();
                            break;
                        
                       
                        case nameof(ShowLobbyLeaveInfo):
                            obj.ShowLobbyLeaveInfo = reader.NextBool();
                            break;
                    }
                }

                reader.EndObject();

                return obj;
            }

            public void ToMValue(PlayerInfoSettings value, IMValueWriter writer)
            {
                writer.BeginObject();

                writer.Name(nameof(ShowCursorInfo));
                writer.Value(value.ShowCursorInfo);

                writer.Name(nameof(ShowLobbyLeaveInfo));
                writer.Value(value.ShowLobbyLeaveInfo);

                writer.EndObject();
            }

            public void ToMValue(object obj, IMValueWriter writer)
            {
                if (obj is PlayerInfoSettings value)
                    ToMValue(value, writer);
            }

            object IMValueBaseAdapter.FromMValue(IMValueReader reader)
                => FromMValue(reader);
        }
    }
}
