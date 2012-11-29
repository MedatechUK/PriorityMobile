namespace J2i.Net.Telnet
{
    partial class ConnectionSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.menuItemOK = new System.Windows.Forms.MenuItem();
            this.menuItemCancel = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.chkBufferedInput = new System.Windows.Forms.CheckBox();
            this.chkLocalEcho = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlNewLineSequence = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConnectionName = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblTerminalType = new System.Windows.Forms.Label();
            this.ddlTerminalType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.menuItemOK);
            this.mainMenu.MenuItems.Add(this.menuItemCancel);
            // 
            // menuItemOK
            // 
            this.menuItemOK.Text = "&OK";
            this.menuItemOK.Click += new System.EventHandler(this.menuItemOK_Click);
            // 
            // menuItemCancel
            // 
            this.menuItemCancel.Text = "&Cancel";
            this.menuItemCancel.Click += new System.EventHandler(this.menuItemCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.Text = "Address";
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.Location = new System.Drawing.Point(3, 60);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(234, 21);
            this.txtAddress.TabIndex = 2;
            this.txtAddress.Text = "towel.blinkenlights.nl";
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(3, 90);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(41, 18);
            this.lblPort.Text = "Port";
            // 
            // chkBufferedInput
            // 
            this.chkBufferedInput.Location = new System.Drawing.Point(124, 85);
            this.chkBufferedInput.Name = "chkBufferedInput";
            this.chkBufferedInput.Size = new System.Drawing.Size(100, 23);
            this.chkBufferedInput.TabIndex = 4;
            this.chkBufferedInput.Text = "Buffered Input";
            // 
            // chkLocalEcho
            // 
            this.chkLocalEcho.Checked = true;
            this.chkLocalEcho.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLocalEcho.Location = new System.Drawing.Point(124, 108);
            this.chkLocalEcho.Name = "chkLocalEcho";
            this.chkLocalEcho.Size = new System.Drawing.Size(100, 20);
            this.chkLocalEcho.TabIndex = 5;
            this.chkLocalEcho.Text = "Local Echo";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 23);
            this.label2.Text = "New Line Sequence";
            // 
            // ddlNewLineSequence
            // 
            this.ddlNewLineSequence.Items.Add("CR");
            this.ddlNewLineSequence.Items.Add("LF");
            this.ddlNewLineSequence.Items.Add("CR+LF");
            this.ddlNewLineSequence.Location = new System.Drawing.Point(121, 134);
            this.ddlNewLineSequence.Name = "ddlNewLineSequence";
            this.ddlNewLineSequence.Size = new System.Drawing.Size(100, 22);
            this.ddlNewLineSequence.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 20);
            this.label3.Text = "Connection Name";
            // 
            // txtConnectionName
            // 
            this.txtConnectionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionName.Location = new System.Drawing.Point(3, 20);
            this.txtConnectionName.Name = "txtConnectionName";
            this.txtConnectionName.Size = new System.Drawing.Size(234, 21);
            this.txtConnectionName.TabIndex = 1;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(50, 87);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(68, 21);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "23";
            // 
            // lblTerminalType
            // 
            this.lblTerminalType.Location = new System.Drawing.Point(3, 168);
            this.lblTerminalType.Name = "lblTerminalType";
            this.lblTerminalType.Size = new System.Drawing.Size(100, 20);
            this.lblTerminalType.Text = "Terminal Type";
            // 
            // ddlTerminalType
            // 
            this.ddlTerminalType.Items.Add("Character Buffer");
            this.ddlTerminalType.Items.Add("ECMA 48");
            this.ddlTerminalType.Location = new System.Drawing.Point(121, 163);
            this.ddlTerminalType.Name = "ddlTerminalType";
            this.ddlTerminalType.Size = new System.Drawing.Size(100, 22);
            this.ddlTerminalType.TabIndex = 7;
            // 
            // ConnectionSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 188);
            this.Controls.Add(this.ddlTerminalType);
            this.Controls.Add(this.lblTerminalType);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtConnectionName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ddlNewLineSequence);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkLocalEcho);
            this.Controls.Add(this.chkBufferedInput);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "ConnectionSettingsForm";
            this.Text = "Edit Connection Settings";
            this.Load += new System.EventHandler(this.ConnectionSettingsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.CheckBox chkBufferedInput;
        private System.Windows.Forms.CheckBox chkLocalEcho;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuItem menuItemOK;
        private System.Windows.Forms.MenuItem menuItemCancel;
        private System.Windows.Forms.ComboBox ddlNewLineSequence;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtConnectionName;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblTerminalType;
        private System.Windows.Forms.ComboBox ddlTerminalType;
    }
}