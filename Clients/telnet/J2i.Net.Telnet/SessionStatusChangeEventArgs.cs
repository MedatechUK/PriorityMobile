using System;
using System.Collections.Generic;
using System.Text;

namespace J2i.Net.Telnet
{
    /// <summary>
    /// Used to notify the client that there is
    /// a change in the connection status
    /// </summary>
    class SessionStatusChangeEventArgs:EventArgs
    {
        public enum SessionStatus { Established, Terminated };
        public SessionStatus ChangedSessionStatus;
    }
}
