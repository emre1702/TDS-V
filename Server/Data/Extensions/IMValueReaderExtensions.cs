using AltV.Net;
using System;

namespace TDS_Server.Data.Extensions
{
    public static class IMValueReaderExtensions
    {

        public static float NextFloat(this IMValueReader reader)
        {
            return (float)reader.NextDouble();
        }
    }
}
