using System;
using System.Collections.Generic;
using System.Text;

namespace J2i.Net.Telnet
{
    /// <summary>
    /// Used in notifying a client that there have been changes in the connection settings. 
    /// </summary>
    public class ConnectionSettingsUpdatedEventArgs:EventArgs
    {


        public UpdatedFieldType UpdatedField = UpdatedFieldType.None;

        public ConnectionSettingsUpdatedEventArgs() { }
        public ConnectionSettingsUpdatedEventArgs(UpdatedFieldType t)
        {
            this.UpdatedField = t;
        }
    }
}
