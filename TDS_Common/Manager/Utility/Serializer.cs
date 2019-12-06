﻿using MessagePack;

namespace TDS_Common.Manager.Utility
{
    public static class Serializer
    {
        // Seems like LZ4MessagePackSerializer and MessagePackSerializer do the same, dunno what the difference is

        public static string ToClient<T>(T obj)
        {
            return LZ4MessagePackSerializer.ToJson(obj);
        }

        public static string ToServer<T>(T obj)
        {
            return ToClient(obj);
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