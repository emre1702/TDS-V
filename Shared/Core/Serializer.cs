using Newtonsoft.Json;

namespace TDS_Shared.Core
{
    public class Serializer
    {

        public string ToClient<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public string ToServer<T>(T obj)
        {
            return ToClient(obj);
        }

        public string ToBrowser<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T FromClient<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T FromServer<T>(string json)
        {
            return FromClient<T>(json);
        }

        public T FromBrowser<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
