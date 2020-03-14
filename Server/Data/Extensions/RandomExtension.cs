using System;

namespace TDS_Server.Data.Extensions
{
    public static class RandomExtension
    {
        public static float NextFloat(this Random random, float lower, float upper)
        {
            return (float)(lower + random.NextDouble() * (upper - lower));
        }
    }
}
