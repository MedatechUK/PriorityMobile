using System;
using System.Collections.Generic;
using System.Text;

namespace J2i.Net.Telnet
{
    public static class Utility
    {
        /// <summary>
        /// Copies information between objects inheriting the IConnectionSettings interface
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyConnectionSettings(IConnectionSettings source, IConnectionSettings destination)
        {
            destination.BufferedInput = source.BufferedInput;
            destination.ConnectionAddress = source.ConnectionAddress;
            destination.ConnectionName = source.ConnectionName;
            destination.LocalEcho = source.LocalEcho;
            destination.NewLineSequence = source.NewLineSequence;
            destination.Port = source.Port;
            destination.TerminalType = source.TerminalType;
        }
    }
}
