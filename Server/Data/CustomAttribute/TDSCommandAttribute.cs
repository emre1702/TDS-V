using System;

namespace TDS.Server.Data.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TDSCommandAttribute : Attribute
    {
        public string Command { get; }
        public int Priority { get; }

        public TDSCommandAttribute(string command, int priority = 0)
        {
            Command = command;
            Priority = priority;
        }
    }
}
