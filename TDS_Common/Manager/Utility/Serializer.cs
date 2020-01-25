using Newtonsoft.Json;

namespace TDS_Common.Manager.Utility
{
    public static class Serializer
    {

        public static string ToClient<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string ToServer<T>(T obj)
        {
            return ToClient(obj);
        }

        public static string ToBrowser<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T FromClient<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T FromServer<T>(string json)
        {
            return FromClient<T>(json);
        }

        public static T FromBrowser<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
