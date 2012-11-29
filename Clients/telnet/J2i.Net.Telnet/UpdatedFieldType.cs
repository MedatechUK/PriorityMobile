using System;
using System.Collections.Generic;
using System.Text;

namespace J2i.Net.Telnet
{
    /// <summary>
    /// List all of the fields that are part of the IConnectionSettings interface. 
    /// Used to generate events when a connection setting is updated.
    /// </summary>
    [Flags()]
    public enum UpdatedFieldType
    {
        None = 0,
        ConnectionName = 1,
        ConnectionAddress = 2,
        BufferedInput = 4,
        Port = 8,
        LocalEcho = 16,
        NewLineSequence = 32,
        TerminalType=64
    }
}
