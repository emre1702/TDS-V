using System;

namespace TDS_Server.Data.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TDSCommand : Attribute
    {
        public string Command;
        public int Priority = 0;

        public TDSCommand(string command, int priority = 0)
        {
            Command = command;
            Priority = priority;
        }

    }
}
