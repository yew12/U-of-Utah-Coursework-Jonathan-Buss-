namespace ChatServer
{
    partial class ChatServerGUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ParticipantsLabel = new System.Windows.Forms.Label();
            this.ServerNameLabel = new System.Windows.Forms.Label();
            this.IPAddressLabel = new System.Windows.Forms.Label();
            this.ShutdownServerButton = new System.Windows.Forms.Button();
            this.ServerNameTextbox = new System.Windows.Forms.TextBox();
            this.ServerIPTextbox = new System.Windows.Forms.TextBox();
            this.MessageTextBox = new System.Windows.Forms.ListBox();
            this.ParticipantsTextbox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ParticipantsLabel
            // 
            this.ParticipantsLabel.AutoSize = true;
            this.ParticipantsLabel.Location = new System.Drawing.Point(76, 98);
            this.ParticipantsLabel.Name = "ParticipantsLabel";
            this.ParticipantsLabel.Size = new System.Drawing.Size(135, 32);
            this.ParticipantsLabel.TabIndex = 0;
            this.ParticipantsLabel.Text = "Participants";
            this.ParticipantsLabel.Click += new System.EventHandler(this.ParticipantsLabel_Click);
            // 
            // ServerNameLabel
            // 
            this.ServerNameLabel.AutoSize = true;
            this.ServerNameLabel.Location = new System.Drawing.Point(952, 22);
            this.ServerNameLabel.Name = "ServerNameLabel";
            this.ServerNameLabel.Size = new System.Drawing.Size(148, 32);
            this.ServerNameLabel.TabIndex = 1;
            this.ServerNameLabel.Text = "Server name";
            this.ServerNameLabel.Click += new System.EventHandler(this.ServerNameLabel_Click);
            // 
            // IPAddressLabel
            // 
            this.IPAddressLabel.AutoSize = true;
            this.IPAddressLabel.Location = new System.Drawing.Point(952, 98);
            this.IPAddressLabel.Name = "IPAddressLabel";
            this.IPAddressLabel.Size = new System.Drawing.Size(198, 32);
            this.IPAddressLabel.TabIndex = 2;
            this.IPAddressLabel.Text = "Server IP Address";
            this.IPAddressLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // ShutdownServerButton
            // 
            this.ShutdownServerButton.Location = new System.Drawing.Point(76, 936);
            this.ShutdownServerButton.Name = "ShutdownServerButton";
            this.ShutdownServerButton.Size = new System.Drawing.Size(606, 46);
            this.ShutdownServerButton.TabIndex = 5;
            this.ShutdownServerButton.Text = "Shutdown Server";
            this.ShutdownServerButton.UseVisualStyleBackColor = true;
            this.ShutdownServerButton.Click += new System.EventHandler(this.ShutdownServerButton_Click);
            // 
            // ServerNameTextbox
            // 
            this.ServerNameTextbox.Location = new System.Drawing.Point(1186, 19);
            this.ServerNameTextbox.Name = "ServerNameTextbox";
            this.ServerNameTextbox.Size = new System.Drawing.Size(426, 39);
            this.ServerNameTextbox.TabIndex = 6;
            this.ServerNameTextbox.TextChanged += new System.EventHandler(this.ServerNameTextbox_TextChanged);
            // 
            // ServerIPTextbox
            // 
            this.ServerIPTextbox.Location = new System.Drawing.Point(1186, 91);
            this.ServerIPTextbox.Name = "ServerIPTextbox";
            this.ServerIPTextbox.Size = new System.Drawing.Size(426, 39);
            this.ServerIPTextbox.TabIndex = 7;
            this.ServerIPTextbox.TextChanged += new System.EventHandler(this.ServerIPTextbox_TextChanged);
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.FormattingEnabled = true;
            this.MessageTextBox.ItemHeight = 32;
            this.MessageTextBox.Location = new System.Drawing.Point(952, 176);
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(717, 772);
            this.MessageTextBox.TabIndex = 8;
            // 
            // ParticipantsTextbox
            // 
            this.ParticipantsTextbox.FormattingEnabled = true;
            this.ParticipantsTextbox.ItemHeight = 32;
            this.ParticipantsTextbox.Location = new System.Drawing.Point(83, 163);
            this.ParticipantsTextbox.Name = "ParticipantsTextbox";
            this.ParticipantsTextbox.Size = new System.Drawing.Size(599, 740);
            this.ParticipantsTextbox.TabIndex = 9;
            // 
            // ChatServerGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1692, 1016);
            this.Controls.Add(this.ParticipantsTextbox);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.ServerIPTextbox);
            this.Controls.Add(this.ServerNameTextbox);
            this.Controls.Add(this.ShutdownServerButton);
            this.Controls.Add(this.IPAddressLabel);
            this.Controls.Add(this.ServerNameLabel);
            this.Controls.Add(this.ParticipantsLabel);
            this.Name = "ChatServerGUI";
            this.Text = "ChatServer";
            this.Load += new System.EventHandler(this.ChatServerGUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label ParticipantsLabel;
        private Label ServerNameLabel;
        private Label IPAddressLabel;
        private Button ShutdownServerButton;
        private TextBox ServerNameTextbox;
        private TextBox ServerIPTextbox;
        private ListBox MessageTextBox;
        private ListBox ParticipantsTextbox;
    }
}