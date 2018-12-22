using System;

namespace TDS_Server.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Parameter)]
    class TDSRemainingText : Attribute
    {
    }
}
