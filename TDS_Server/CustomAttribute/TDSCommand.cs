using System;

namespace TDS_Server.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method)]
    class TDSCommand : Attribute
    {
        public string Command;

        public TDSCommand(string command) => this.Command = command;
    }
}
