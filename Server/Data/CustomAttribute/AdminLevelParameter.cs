using System;

namespace TDS.Server.Data.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AdminLevelParameterAttribute : Attribute
    {
    }
}
