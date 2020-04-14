using Newtonsoft.Json;
using System;

namespace TDS_Shared.Core
{
    public class Serializer
    {
        private readonly Action<string> _infoLogger;
        private readonly Action<Exception> _errorLogger;

        public Serializer() { }

        public Serializer(Action<string> infoLogger, Action<Exception> errorLogger)
        {
            _infoLogger = infoLogger;
            _errorLogger = errorLogger;
        } 

        public string ToClient<T>(T obj)
        {
            try
            {
                _infoLogger?.Invoke("Start ToClient Serializer");
                var json = JsonConvert.SerializeObject(obj);
                _infoLogger?.Invoke("End ToClient Serializer");

                return json;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return "[]";
            }
        }

        public string ToServer<T>(T obj)
        {
            try
            {
                _infoLogger?.Invoke("Start ToServer Serializer");
                var json = JsonConvert.SerializeObject(obj);
                _infoLogger?.Invoke("End ToServer Serializer");

                return json;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return "[]";
            }
        }

        public string ToBrowser<T>(T obj)
        {
            try
            {
                _infoLogger?.Invoke("Start ToBrowser Serializer");
                var json = JsonConvert.SerializeObject(obj);
                _infoLogger?.Invoke("End ToBrowser Serializer");

                return json;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return "[]";
            }
        }

        public T FromClient<T>(string json)
        {
            try
            {
                _infoLogger?.Invoke("Start FromClient Serializer");
                var obj = JsonConvert.DeserializeObject<T>(json);
                _infoLogger?.Invoke("End FromClient Serializer");

                return obj;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return default;
            }
        }

        public T FromServer<T>(string json)
        {
            try
            {
                _infoLogger?.Invoke("Start FromServer Serializer");
                var obj = JsonConvert.DeserializeObject<T>(json);
                _infoLogger?.Invoke("End FromServer Serializer");

                return obj;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return default;
            }
        }

        public T FromBrowser<T>(string json)
        {
            try
            {
                _infoLogger?.Invoke("Start FromBrowser Serializer");
                var obj = JsonConvert.DeserializeObject<T>(json);
                _infoLogger?.Invoke("End FromBrowser Serializer");

                return obj;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return default;
            }
        }
    }
}
