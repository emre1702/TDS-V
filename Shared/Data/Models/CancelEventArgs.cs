using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Shared.Data.Models
{
    public class CancelEventArgs
    {
        public bool Cancel { get; set; }

        public CancelEventArgs() { }
        public CancelEventArgs(bool cancel) => Cancel = cancel;

        
    }
}
