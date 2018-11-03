using System;

namespace TDS.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method)]
    class TDSCommand : Attribute
    {
        public string Command;

        public TDSCommand(string command) => this.Command = command;
    }
}
