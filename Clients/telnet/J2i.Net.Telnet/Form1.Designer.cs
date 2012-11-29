namespace J2i.Net.Telnet
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemNewConnection = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemTelnetGoAhead = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItemRestartSession = new System.Windows.Forms.MenuItem();
            this.menuItemCloseSession = new System.Windows.Forms.MenuItem();
            this.menuItemClearScreen = new System.Windows.Forms.MenuItem();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem6);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItemNewConnection);
            this.menuItem1.MenuItems.Add(this.menuItemSave);
            this.menuItem1.MenuItems.Add(this.menuItemOpen);
            this.menuItem1.MenuItems.Add(this.menuItem5);
            this.menuItem1.MenuItems.Add(this.menuItemExit);
            this.menuItem1.Text = "&File";
            // 
            // menuItemNewConnection
            // 
            this.menuItemNewConnection.Text = "&New Connection";
            this.menuItemNewConnection.Click += new System.EventHandler(this.menuItemNewConnection_Click);
            // 
            // menuItemSave
            // 
            this.menuItemSave.Text = "&Save";
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Text = "&Open";
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Text = "-";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.MenuItems.Add(this.menuItem2);
            this.menuItem6.MenuItems.Add(this.menuItem7);
            this.menuItem6.MenuItems.Add(this.menuItemRestartSession);
            this.menuItem6.MenuItems.Add(this.menuItemCloseSession);
            this.menuItem6.MenuItems.Add(this.menuItemClearScreen);
            this.menuItem6.Text = "Session";
            // 
            // menuItem2
            // 
            this.menuItem2.MenuItems.Add(this.menuItem3);
            this.menuItem2.MenuItems.Add(this.menuItem4);
            this.menuItem2.MenuItems.Add(this.menuItemTelnetGoAhead);
            this.menuItem2.Text = "Command";
            // 
            // menuItem3
            // 
            this.menuItem3.Text = "NOP";
            // 
            // menuItem4
            // 
            this.menuItem4.Text = "Are You There";
            // 
            // menuItemTelnetGoAhead
            // 
            this.menuItemTelnetGoAhead.Text = "Go Ahead";
            // 
            // menuItem7
            // 
            this.menuItem7.Text = "-";
            // 
            // menuItemRestartSession
            // 
            this.menuItemRestartSession.Text = "Restart Session";
            this.menuItemRestartSession.Click += new System.EventHandler(this.menuItemRestartSession_Click);
            // 
            // menuItemCloseSession
            // 
            this.menuItemCloseSession.Text = "Close Session";
            this.menuItemCloseSession.Click += new System.EventHandler(this.menuItemCloseSession_Click);
            // 
            // menuItemClearScreen
            // 
            this.menuItemClearScreen.Text = "Clear Screen";
            this.menuItemClearScreen.Click += new System.EventHandler(this.menuItemClearScreen_Click);
            // 
            // txtInput
            // 
            this.txtInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtInput.Location = new System.Drawing.Point(0, 247);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(240, 21);
            this.txtInput.TabIndex = 3;
            this.txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInput_KeyPress);
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.White;
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular);
            this.txtOutput.ForeColor = System.Drawing.Color.Navy;
            this.txtOutput.Location = new System.Drawing.Point(0, 0);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(240, 247);
            this.txtOutput.TabIndex = 6;
            this.txtOutput.WordWrap = false;
            this.txtOutput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOutput_KeyPress);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.txtInput);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Telnet";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemNewConnection;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.MenuItem menuItemSave;
        private System.Windows.Forms.MenuItem menuItemOpen;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItemTelnetGoAhead;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItemRestartSession;
        private System.Windows.Forms.MenuItem menuItemCloseSession;
        private System.Windows.Forms.MenuItem menuItemClearScreen;
    }
}

