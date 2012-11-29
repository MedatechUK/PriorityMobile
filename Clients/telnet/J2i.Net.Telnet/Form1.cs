using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml.Serialization;

namespace J2i.Net.Telnet
{
    public partial class Form1 : Form
    {
        delegate void VoidDelegate();

        VoidDelegate ShowTerminalBufferDelegate;
        TelnetSession currentSession = null;
        EventHandler<BlockReceivedEventArgs> blockReceivedHandler;
        EventHandler<SessionStatusChangeEventArgs> sessionStatusHandler;
        CConnectionSettings _connectionSettings = new CConnectionSettings();
        TerminalEmulator terminalBuffer;
        int _maxBufferSize = (4 * 1024);
        StringBuilder sbOutputBuffer ;
               

        public Form1()
        {
            InitializeComponent();
            blockReceivedHandler = new EventHandler<BlockReceivedEventArgs>(currentSession_BlockReceived);
            sessionStatusHandler = new EventHandler<SessionStatusChangeEventArgs>(currentSession_SessionStatusChanged);
            _connectionSettings.ConnectionSettingsUpdated+=new EventHandler(_connectionSettings_ConnectionSettingsUpdated);
            terminalBuffer = new TerminalEmulator();
            ShowTerminalBufferDelegate = new VoidDelegate(this.ShowTerminalBuffer);
            sbOutputBuffer = new StringBuilder(_maxBufferSize);
        }

        void  _connectionSettings_ConnectionSettingsUpdated(object sender, EventArgs e)
        {
 	        
        }

        private void txtOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //TelnetSession ts = new TelnetSession("alcedes.com", 80);
            //ts.Send("GET / \r\n\r\n");
            //System.Threading.Thread.Sleep(2000);
            //string result = ts.ReadLine();
            //System.Threading.Thread.Sleep(2000);
            this.UpdateUILayout();
        }

        void CloseSession()
        {
            if(currentSession!=null)
            {

                currentSession.BlockReceived-=blockReceivedHandler;
                currentSession.Dispose();
                currentSession = null;
                
            }
        }
        void OpenSession(IConnectionSettings connectionSettings)
        {
            if (currentSession != null)
            {
                CloseSession();
            }
            try
            {
                Utility.CopyConnectionSettings(connectionSettings, _connectionSettings);
                currentSession = new TelnetSession(connectionSettings.ConnectionAddress, connectionSettings.Port);
                currentSession.BlockReceived += blockReceivedHandler;
                currentSession.SessionStatusChanged += sessionStatusHandler;
                this.UpdateUILayout();
                
            }
            catch (Exception exc)
            {
                if (_connectionSettings.TerminalType==TerminalType.CharacterBuffer)
                {
                    AppendText(exc.Message + "\r\n");
                    MoveCursorToEnd();
                }
                else
                {
                    terminalBuffer.Write(exc.Message + "\r\n");
                    ShowTerminalBuffer();
                }                
                this.currentSession = null;
            }
            
            
        }

        void currentSession_SessionStatusChanged(object sender, SessionStatusChangeEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(this.sessionStatusHandler, new object[] { sender, e });
            }
            else
            {
                if (_connectionSettings.TerminalType==TerminalType.CharacterBuffer)
                {
                    AppendText("\r\n\r\nNew Session Established\r\n\r\n");
                    MoveCursorToEnd();
                    
                }
                else
                {
                    this.terminalBuffer.Write("New Session Established\r\n\r\n");
                    ShowTerminalBuffer();
                }
            }
        }

        void currentSession_BlockReceived(object sender, BlockReceivedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(blockReceivedHandler, new object[] { sender, e });
            }
            else
            {
                string receivedBlock = currentSession.ReadBlock();
                /*
                if (_connectionSettings.TerminalType==TerminalType.CharacterBuffer)
                {
                    if (receivedBlock.IndexOf("\x1b\x5bJ") != -1)
                    {
                        ClearText();
                        receivedBlock = receivedBlock.Substring(receivedBlock.IndexOf("\x1b\x5bJ") + 1);
                    }
                   
                    AppendText(receivedBlock);
                    MoveCursorToEnd();
                }
                else*/
                {
                    terminalBuffer.Write(receivedBlock);
                    ShowTerminalBuffer();
                }
            }
        }
        private void menuItemNewConnection_Click(object sender, EventArgs e)
        {
            ConnectionSettingsForm connectionSettingsForm = new ConnectionSettingsForm();
            Utility.CopyConnectionSettings(this._connectionSettings, connectionSettingsForm);
            if (connectionSettingsForm.ShowDialog() == DialogResult.OK)
            {
               
                
                Utility.CopyConnectionSettings(connectionSettingsForm, _connectionSettings);
                OpenSession(_connectionSettings);

            }
        }

        void Send(string message)
        {
            if (currentSession != null)
            {
                if (this._connectionSettings.LocalEcho)
                {
                    if (_connectionSettings.TerminalType==TerminalType.CharacterBuffer)
                    {
                        AppendText(message);
                    }
                    else
                    {
                        terminalBuffer.Write(message);
                        ShowTerminalBuffer();
                    }
                }
                try
                {
                    this.currentSession.Send(message);
                }
                catch (System.Net.Sockets.SocketException socketExc)
                {
                    string msg = String.Format("\r\n\r\n{0}\r\n\r\n", socketExc.Message);
                    if (_connectionSettings.TerminalType==TerminalType.CharacterBuffer)
                    {
                        AppendText( msg);
                    }
                    else
                    {
                        terminalBuffer.Write(msg);
                        ShowTerminalBuffer();
                    }
                }
            }

        }




        void toggleEchoStatus()
        {
            _connectionSettings.LocalEcho= (!_connectionSettings.LocalEcho);
            UpdateUILayout();
        }
        void toggleBufferedStatus()
        {
            this._connectionSettings.BufferedInput=(!_connectionSettings.BufferedInput);
            UpdateUILayout();
        }

        void UpdateUILayout()
        {

            this.txtInput.Visible = _connectionSettings.BufferedInput; 
            this.Text = String.Format("Telnet : {0}", this._connectionSettings.ConnectionName);
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItemSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd;
            
            string targetFileName = string.Empty;
            try
            {
                sfd = new SaveFileDialog();
                sfd.Filter = "Connection File (*.telnet)|*.telnet";
                if (DialogResult.OK == sfd.ShowDialog())
                {
                    targetFileName = sfd.FileName;
                }
            }
            catch (NotSupportedException exc)
            {
                targetFileName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) +"\\"+ _connectionSettings.ConnectionName;
            }
            if (targetFileName.Length>0)
            {
                using (StreamWriter sw = new StreamWriter(targetFileName))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(CConnectionSettings));
                    xs.Serialize(sw, this._connectionSettings);
                    sw.Close();
                }
            }
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Connection File(*.telnet)|*.telnet";
            if (DialogResult.OK == ofd.ShowDialog())
            {
                using (StreamReader sr = new StreamReader(ofd.FileName))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(CConnectionSettings));
                    IConnectionSettings con = (IConnectionSettings)xs.Deserialize(sr);
                    this.OpenSession(con);
                }
            }
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar=='\r')
            {
                Send(txtInput.Text+_connectionSettings.NewLineSequence);
                txtInput.Text=String.Empty;
                e.Handled = true;
            }
        }

        private void txtOutput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!_connectionSettings.BufferedInput)
            {
                switch(e.KeyChar)
                {
                    case '\r':
                        Send(_connectionSettings.NewLineSequence);
                        break;
                    default:
                        Send(e.KeyChar.ToString());
                        break;
                }
                e.Handled=true;
            }
        }

        private void menuItemTelnetGoAhead_Click(object sender, EventArgs e)
        {
            this.Send("\xF6\xFF");
        }

        void ShowTerminalBuffer()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(ShowTerminalBufferDelegate);
            }
            else
            {
                string output = terminalBuffer.ToString();
                //System.Diagnostics.Debug.WriteLine(output);
                this.txtOutput.Text = output;
                MoveCursorToEnd();
                System.Diagnostics.Debug.WriteLine(sbOutputBuffer.ToString(), "txtOutput");                
            }
        }

        void MoveCursorToEnd()
        {
            this.txtOutput.Select(this.txtOutput.Text.Length, 0);
        }
        void AppendText(String s)
        {
            sbOutputBuffer.Append(s);
            if(sbOutputBuffer.Length>_maxBufferSize)
            {
                sbOutputBuffer.Remove(0,sbOutputBuffer.Length-_maxBufferSize);
            }
            this.txtOutput.Text=sbOutputBuffer.ToString();
            MoveCursorToEnd();
        }
        void ClearText()
        {
            this.sbOutputBuffer.Length=0;
        }

        private void menuItemRestartSession_Click(object sender, EventArgs e)
        {
            if (this._connectionSettings != null)
            {
                OpenSession(this._connectionSettings);
            }
        }

        private void menuItemCloseSession_Click(object sender, EventArgs e)
        {
            CloseSession();
        }

        private void menuItemClearScreen_Click(object sender, EventArgs e)
        {
            if (this._connectionSettings.TerminalType == TerminalType.CharacterBuffer)
            {
                ClearText();
                this.txtOutput.Text = String.Empty;
            }
            else
            {
                this.terminalBuffer.Clear();
                ShowTerminalBuffer();
            }
        }
    }
}