using System;

namespace TDS_Server.Database.Entity.Log
{
    public partial class LogErrors
    {
        #region Public Properties

        public string ExceptionType { get; set; }
        public long Id { get; set; }
        public string Info { get; set; }
        public int? Source { get; set; }
        public string StackTrace { get; set; }
        public DateTime Timestamp { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"[{Timestamp.ToString()}] {ExceptionType} error (source = {Source?.ToString() ?? "?"}) :{Environment.NewLine}{Info}{Environment.NewLine}{StackTrace}";
        }

        #endregion Public Methods
    }
}
