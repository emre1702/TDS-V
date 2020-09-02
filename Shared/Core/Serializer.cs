using System;
using MessagePack;

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

        public byte[] ToClient<T>(T obj)
        {
            try
            {
                _infoLogger?.Invoke("Start ToClient Serializer");
                var json = MessagePackSerializer.Serialize(obj);
                _infoLogger?.Invoke("End ToClient Serializer");

                return json;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return Array.Empty<byte>();
            }
        }

        public byte[] ToServer<T>(T obj)
        {
            try
            {
                _infoLogger?.Invoke("Start ToServer Serializer");
                var bytes = MessagePackSerializer.Serialize(obj);
                _infoLogger?.Invoke("End ToServer Serializer");

                return bytes;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return Array.Empty<byte>();
            }
        }

        public byte[] ToBrowser<T>(T obj)
        {
            try
            {
                _infoLogger?.Invoke("Start ToBrowser Serializer");
                var bytes = MessagePackSerializer.Serialize(obj);
                _infoLogger?.Invoke("End ToBrowser Serializer");

                return bytes;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return Array.Empty<byte>();
            }
        }

        public T FromClient<T>(byte[] bytes)
        {
            try
            {
                _infoLogger?.Invoke("Start FromClient Serializer");
                var obj = MessagePackSerializer.Deserialize<T>(bytes);
                _infoLogger?.Invoke("End FromClient Serializer");

                return obj;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return default;
            }
        }

        public T FromServer<T>(byte[] bytes)
        {
            try
            {
                _infoLogger?.Invoke("Start FromServer Serializer");
                var obj = MessagePackSerializer.Deserialize<T>(bytes);
                _infoLogger?.Invoke("End FromServer Serializer");

                return obj;
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex);
                return default;
            }
        }

        public T FromBrowser<T>(byte[] bytes)
        {
            try
            {
                _infoLogger?.Invoke("Start FromBrowser Serializer");
                var obj = MessagePackSerializer.Deserialize<T>(bytes);
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
