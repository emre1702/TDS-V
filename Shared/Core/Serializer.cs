using Newtonsoft.Json;
using System;

namespace TDS.Shared.Core
{
    public static class Serializer
    {
        private static Action<string> _infoLogger;
        private static Action<Exception> _errorLogger;

        private static readonly JsonSerializerSettings _settingsIgnoreDefaultValues = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        private static readonly JsonSerializerSettings _settingsUseDefaultValues = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Populate
        };

        public static void Init(Action<string> infoLogger, Action<Exception> errorLogger)
        {
            _infoLogger = infoLogger;
            _errorLogger = errorLogger;
        }

        public static string ToClient<T>(T obj, bool ignoreDefaultValues = true)
        {
            try
            {
                _infoLogger?.Invoke("Start ToClient Serializer");
                var settings = ignoreDefaultValues ? _settingsIgnoreDefaultValues : _settingsUseDefaultValues;
                var json = JsonConvert.SerializeObject(obj, settings);
                _infoLogger?.Invoke("End ToClient Serializer");

                return json;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return "[]";
            }
        }

        public static string ToServer<T>(T obj, bool ignoreDefaultValues = true)
        {
            try
            {
                _infoLogger?.Invoke("Start ToServer Serializer");
                var settings = ignoreDefaultValues ? _settingsIgnoreDefaultValues : _settingsUseDefaultValues;
                var json = JsonConvert.SerializeObject(obj, settings);
                _infoLogger?.Invoke("End ToServer Serializer");

                return json;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return "[]";
            }
        }

        public static string ToBrowser<T>(T obj, bool ignoreDefaultValues = false)
        {
            try
            {
                _infoLogger?.Invoke("Start ToBrowser Serializer");
                var settings = ignoreDefaultValues ? _settingsIgnoreDefaultValues : _settingsUseDefaultValues;
                var json = JsonConvert.SerializeObject(obj, settings);
                _infoLogger?.Invoke("End ToBrowser Serializer");

                return json;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return "[]";
            }
        }

        public static T FromClient<T>(string json, bool ignoreDefaultValues = true)
        {
            try
            {
                _infoLogger?.Invoke("Start FromClient Serializer");
                var settings = ignoreDefaultValues ? _settingsIgnoreDefaultValues : _settingsUseDefaultValues;
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

        public static T FromServer<T>(string json, bool ignoreDefaultValues = true)
        {
            try
            {
                _infoLogger?.Invoke("Start FromServer Serializer");
                var settings = ignoreDefaultValues ? _settingsIgnoreDefaultValues : _settingsUseDefaultValues;
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

        public static T FromBrowser<T>(string json, bool ignoreDefaultValues = true)
        {
            try
            {
                _infoLogger?.Invoke("Start FromBrowser Serializer");
                var settings = ignoreDefaultValues ? _settingsIgnoreDefaultValues : _settingsUseDefaultValues;
                var obj = JsonConvert.DeserializeObject<T>(json, settings);
                _infoLogger?.Invoke("End FromBrowser Serializer");

                return obj;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return default;
            }
        }

        public static object FromBrowser(Type type, string json, bool ignoreDefaultValues = true)
        {
            try
            {
                _infoLogger?.Invoke("Start FromBrowser Serializer");
                var settings = ignoreDefaultValues ? _settingsIgnoreDefaultValues : _settingsUseDefaultValues;
                var obj = JsonConvert.DeserializeObject(json, type, settings);
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