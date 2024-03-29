﻿using System;

namespace TDS.Shared.Data.Extensions
{
    public static class RandomExtension
    {
        public static float NextFloat(this Random random, float lower, float upper)
        {
            return (float)(lower + random.NextDouble() * (upper - lower));
        }
    }
}
