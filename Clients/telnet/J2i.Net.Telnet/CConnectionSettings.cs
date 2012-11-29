using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
//using System.Runtime.

namespace J2i.Net.Telnet
{
    //[Serializable()]
    public class CConnectionSettings : IConnectionSettings
    {

        public event EventHandler ConnectionSettingsUpdated;

        protected void OnConnectionSettingsUpdated(ConnectionSettingsUpdatedEventArgs e)
        {
            if (this.ConnectionSettingsUpdated != null)
            {
                this.ConnectionSettingsUpdated(this, e);
            }
        }
        #region IConnectionSettings Members


        //[XmlElement("ConnectionName", typeof(string))]
        public string ConnectionName
        {
            get
            {
                return m_connectionName;
            }
            set
            {
                this.m_connectionName = value;
                OnConnectionSettingsUpdated(new ConnectionSettingsUpdatedEventArgs(UpdatedFieldType.ConnectionName));
            }
        }
        string m_connectionName;

        //[XmlElement("ConnectionAddress",typeof(string))]
        public string ConnectionAddress
        {
            get
            {
                return this.m_connectionAddress;
            }
            set
            {
                this.m_connectionAddress=value;
                this.OnConnectionSettingsUpdated(new ConnectionSettingsUpdatedEventArgs(UpdatedFieldType.ConnectionAddress));
            }
        }
        string m_connectionAddress = String.Empty;


        public bool BufferedInput
        {
            get
            {
                return this.m_bufferedInput;
            }
            set
            {
                this.m_bufferedInput = value;
                this.OnConnectionSettingsUpdated(new ConnectionSettingsUpdatedEventArgs(UpdatedFieldType.BufferedInput));
            }
        }
        bool m_bufferedInput=true;

        public int Port
        {
            get { return this.m_port; }
            set 
            { 
                m_port = value;
                this.OnConnectionSettingsUpdated(new ConnectionSettingsUpdatedEventArgs(UpdatedFieldType.Port));
            }
        }
        int m_port=23;

        public bool LocalEcho
        {
            get
            {
                return m_localEcho;
            }
            set
            {
                m_localEcho = value;
                this.OnConnectionSettingsUpdated(new ConnectionSettingsUpdatedEventArgs(UpdatedFieldType.LocalEcho));
            }
        }
        bool m_localEcho = false;

        public string NewLineSequence
        {
            get
            {
                return this.m_newLineSequence;
            }
            set
            {
                this.m_newLineSequence=value;
                this.OnConnectionSettingsUpdated(new ConnectionSettingsUpdatedEventArgs(UpdatedFieldType.NewLineSequence));
            }
        }
        string m_newLineSequence = "\r";


        public TerminalType TerminalType
        {
            get
            {
                return _terminalType;
            }
            set
            {
                _terminalType = value;
            }
        }
        TerminalType _terminalType = TerminalType.CharacterBuffer;

        #endregion
    }
}
