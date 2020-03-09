using System;

namespace TDS_Server.Data.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class TDSCommand : Attribute
    {
        public string Command;
        public int Priority = 0;

        public TDSCommand(string command, int priority = 0)
        {
            this.Command = command;
            this.Priority = priority;
        }
    }
}