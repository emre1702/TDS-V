using MessagePack;

namespace TDS_Common.Manager.Utility
{
    public static class Serializer
    {

        public static string ToClient<T>(T obj)
        {
            return LZ4MessagePackSerializer.ToJson(obj);
        }

        public static string ToBrowser<T>(T obj)
        {
            return MessagePackSerializer.ToJson(obj);
        }

        public static T FromClient<T>(string json)
        {
            var bytes = LZ4MessagePackSerializer.FromJson(json);
            return LZ4MessagePackSerializer.Deserialize<T>(bytes);
        }

        public static T FromServer<T>(string json)
        {
            return FromClient<T>(json);
        }

        public static T FromBrowser<T>(string json)
        {
            var bytes = MessagePackSerializer.FromJson(json);
            return MessagePackSerializer.Deserialize<T>(bytes);
        }
    }
}
