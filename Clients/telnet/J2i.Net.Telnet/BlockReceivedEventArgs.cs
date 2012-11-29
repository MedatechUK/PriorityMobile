using System;
using System.Collections.Generic;
using System.Text;

namespace J2i.Net.Telnet
{
    class BlockReceivedEventArgs:EventArgs
    {
        public int BlockLength
        {
            get { return this._blockLength; }
            set { this._blockLength = value; }
        }
        int _blockLength;

    }
}
