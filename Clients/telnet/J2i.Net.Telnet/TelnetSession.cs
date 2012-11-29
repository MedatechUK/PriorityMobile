using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Security.Permissions;
using System.Threading;



namespace J2i.Net.Telnet
{
    /// <summary>
    /// Initiates a telnet session offering methods to write to the session and notifies the 
    /// client when information is available to be read from the session.  The read information
    /// is queued up for retrieval.
    /// </summary>
    class TelnetSession :IDisposable
    {
        const int readBufferSize = 64*1024;
        string _address;
        int _port;
        Thread _readerThread = null;
        Socket _socket;
        NetworkStream _networkStream;
        StreamReader _readerStream;
        StreamWriter _writerStream;
        Queue<string> _messageQueue = new Queue<string>();
        int _readLines = -1;

        public event EventHandler<BlockReceivedEventArgs> BlockReceived = null;
        public event EventHandler<SessionStatusChangeEventArgs> SessionStatusChanged = null;


        char[] _receivedBuffer = new char[readBufferSize];

        delegate void voidDelegate();

        void Close()
        {
            _readerThread.Abort();
            _socket.Close();
        }

        /// <summary>
        /// Main thread for the worker process that reads from the telnet thread's socket
        /// </summary>
        public void ReaderLoop()
        {
            
            ASCIIEncoding ae = new ASCIIEncoding();
            StringBuilder sb = new StringBuilder();
            Interlocked.Increment(ref _readLines);

            SessionStatusChangeEventArgs args = new SessionStatusChangeEventArgs();
            OnSessionStatusChanged(args);
            args.ChangedSessionStatus = SessionStatusChangeEventArgs.SessionStatus.Established;

            while (true)
            {
                
                try
                {
                    if (_networkStream.DataAvailable)
                    {

                        int readSize = (_socket.Available > readBufferSize) ? readBufferSize : _socket.Available;
                        int readCount = _readerStream.ReadBlock(_receivedBuffer, 0, readSize);
                        if (readCount > 0)
                        {
                            Interlocked.Increment(ref _readLines);
                            sb.Append(_receivedBuffer, 0, readCount);
                            string newValue = sb.ToString();
                            Debug.WriteLine(String.Format("Received Line:{0}", newValue));
                            _messageQueue.Enqueue(newValue);
                            BlockReceivedEventArgs brea = new BlockReceivedEventArgs();
                            brea.BlockLength = sb.Length;
                            sb.Length = 0;
                            OnBlockReceived(brea);
                            
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch (Exception exc) 
                {
                    Debug.WriteLine(exc.Message);                
                }
            }
        }

        /// <summary>
        /// Notifies the client that a block is available to be read
        /// </summary>
        /// <param name="args"></param>
        protected void OnBlockReceived(BlockReceivedEventArgs args)
        {
            if (BlockReceived != null)
            {
                BlockReceived(this, args);
            }
        }

        /// <summary>
        /// Notifies the client when a session is established or terminated
        /// </summary>
        /// <param name="args"></param>
        protected void OnSessionStatusChanged(SessionStatusChangeEventArgs args)
        {
            if (SessionStatusChanged != null)
            {
                SessionStatusChanged(this, args);
            }
        }

        /// <summary>
        /// Retrieves the next text block off the queue and returns 
        /// it to the client.  If nothing is available an empty
        /// string is returned.
        /// </summary>
        /// <returns></returns>
        public string ReadBlock()
        {
            if (_messageQueue.Count > 0)
            {
                return _messageQueue.Dequeue();
            }
            return string.Empty;
        }
        

        /// <summary>
        /// Opens a telnet session to a given address on the specified port
        /// </summary>
        /// <param name="address">the server to which to connect</param>
        /// <param name="port">the port to use for the connection</param>
        public TelnetSession(string address, int port)
        {
            this._address = address;
            this._port = port;
            _socket = CreateSocket(address, port);
            _networkStream = new NetworkStream(_socket);
            _readerStream = new StreamReader(_networkStream);
            _writerStream = new StreamWriter(_networkStream);
            _writerStream.AutoFlush = true;
            ThreadStart ts= new ThreadStart(new voidDelegate(ReaderLoop));
            _readerThread = new Thread(ts);
            _readerThread.IsBackground = true;
            _readerThread.Start();


        }

        /// <summary>
        /// Establishes the socket connection for the telnet session
        /// </summary>
        /// <param name="machineAddress">The machine to which the client must connect</param>
        /// <param name="port">the remote port to use for the connection</param>
        /// <returns></returns>
        Socket CreateSocket(string machineAddress, int port)
        {
            
            //Resolve the IP address(es) of the target machine
            IPHostEntry iphostEntry = null;
            iphostEntry = Dns.GetHostEntry(machineAddress);
            //let's go through the list of IP addresses returned and attempt
            //to connect to each one.  once a successful connection is made
            //return the socket created from that connection and stop testing
            //on any remaining ports.  If no connection can be established then 
            //return null
            foreach (IPAddress address in iphostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address.Address, port);
                Socket tempSocket = new Socket(ipe.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
                tempSocket.Connect(ipe);
                if (tempSocket.Connected)
                {
                    return tempSocket;
                }
            }
            return null;
        }

        /// <summary>
        /// Sends a byte to the remote machine
        /// </summary>
        /// <param name="message"></param>
        public void Send(byte[] message)
        {
            _writerStream.Write(message);
            _writerStream.Flush();
        }

        /// <summary>
        /// Sends a string to the remote machine as an array of bytes
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            byte[] bMessage = System.Text.ASCIIEncoding.ASCII.GetBytes(message);
            this._socket.Send(bMessage);
        }


        #region IDisposable Members

        public void Dispose()
        {
            this.Close();
        }

        #endregion
    }
}
