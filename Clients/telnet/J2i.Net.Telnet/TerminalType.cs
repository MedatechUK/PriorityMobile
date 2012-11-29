using System;
using System.Collections.Generic;
using System.Text;

namespace J2i.Net.Telnet
{

    /// <summary>
    /// Identifies the telnet terminal types that can be emulated with this software
    /// </summary>
    public enum TerminalType:int
    {
        /// <summary>
        /// The CharacterBuffer terminal type simple displays 
        /// the received characters on the screen with no 
        /// further processing
        /// </summary>
        CharacterBuffer,
        /// <summary>
        /// The ECMA48 terminal type is experimental.  Currently the 
        /// terminal will process a subset of escape sequences used in ECMA 48. 
        /// Since this program is for diagnostic purposes I may not fully implement
        /// the ECMA48 terminal
        /// </summary>
        ECMA48 // http://www.ecma-international.org/publications/files/ECMA-ST/Ecma-048.pdf
    }
}
