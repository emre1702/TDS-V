using System;

namespace TDS_Server.Data.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TDSCommand : Attribute
    {
        #region Public Fields

        public string Command;
        public int Priority = 0;

        #endregion Public Fields

        #region Public Constructors

        public TDSCommand(string command, int priority = 0)
        {
            this.Command = command;
            this.Priority = priority;
        }

        #endregion Public Constructors
    }
}
