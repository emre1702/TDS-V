using System;

namespace TDS_Server.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal class TDSRemainingText : Attribute
    {
    }
}