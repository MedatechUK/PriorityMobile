using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace J2i.Net.Telnet
{
    public partial class ConnectionSettingsForm : Form, IConnectionSettings
    {
        string[] LineFeedSequence = new string[] { "\r", "\n", "\r\n" };
        public string ConnectionName
        {
            get { return this.txtConnectionName.Text; }
            set { this.txtConnectionName.Text = value; }
        }
        public string ConnectionAddress
        {
            get { return this.txtAddress.Text; }
            set { this.txtAddress.Text = value; }
        }

        public bool BufferedInput
        {
            get { return this.chkBufferedInput.Checked; }
            set { this.chkBufferedInput.Checked = value; }
        }

        public bool LocalEcho
        {
            get { return this.chkLocalEcho.Checked; }
            set { this.chkLocalEcho.Checked = value; }
        }

        public string NewLineSequence
        {
            get { return LineFeedSequence[this.ddlNewLineSequence.SelectedIndex]; }
            set {
                switch (value)
                {
                    case "\r":
                        this.ddlNewLineSequence.SelectedIndex = 0;
                        break;
                    case "\n":
                        this.ddlNewLineSequence.SelectedIndex = 1;
                        break;
                    default:
                        this.ddlNewLineSequence.SelectedIndex = 2;
                        break;
                }
            }
        }

        public int Port
        {
            get 
            {
                int retVal = 0;
                try
                {
                    retVal = int.Parse(txtPort.Text);
                }
                catch { }
                return retVal; 
            }
            set { this.txtPort.Text= value.ToString(); }
        }

        public TerminalType TerminalType
        {
            get
            {
                if (this.ddlTerminalType.SelectedIndex == 0)
                    return TerminalType.CharacterBuffer;

                return TerminalType.ECMA48;
            }
            set
            {
                if (value == TerminalType.CharacterBuffer)
                    this.ddlTerminalType.SelectedIndex = 0;
                else
                    this.ddlTerminalType.SelectedIndex = 1;
            }
        }

        public ConnectionSettingsForm()
        {
            InitializeComponent();
        }

        private void menuItemOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void menuItemCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ConnectionSettingsForm_Load(object sender, EventArgs e)
        {
            this.ddlNewLineSequence.SelectedIndex = 2;
            this.ddlTerminalType.SelectedIndex = 0;
        }
    }
}