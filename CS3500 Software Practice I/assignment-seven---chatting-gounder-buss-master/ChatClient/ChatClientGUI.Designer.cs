namespace ChatClient
{
    partial class ChatClientGUI
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
            this.ConnectServerButton = new System.Windows.Forms.Button();
            this.ServerNameLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.ServerNameTextbox = new System.Windows.Forms.TextBox();
            this.NameTextbox = new System.Windows.Forms.TextBox();
            this.RetrieveParticipantsButton = new System.Windows.Forms.Button();
            this.TypeMessageLabel = new System.Windows.Forms.Label();
            this.TypeMessageTexbox = new System.Windows.Forms.RichTextBox();
            this.ParticipantsTextBox = new System.Windows.Forms.ListBox();
            this.MessageDisplayTextBox = new System.Windows.Forms.ListBox();
            this.connectedToServerLabel = new System.Windows.Forms.Label();
            this.sendMessageButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnectServerButton
            // 
            this.ConnectServerButton.Location = new System.Drawing.Point(328, 180);
            this.ConnectServerButton.Name = "ConnectServerButton";
            this.ConnectServerButton.Size = new System.Drawing.Size(416, 70);
            this.ConnectServerButton.TabIndex = 0;
            this.ConnectServerButton.Text = "Connect to Server";
            this.ConnectServerButton.UseVisualStyleBackColor = true;
            this.ConnectServerButton.Click += new System.EventHandler(this.ConnectServerButton_Click);
            // 
            // ServerNameLabel
            // 
            this.ServerNameLabel.AutoSize = true;
            this.ServerNameLabel.Location = new System.Drawing.Point(68, 40);
            this.ServerNameLabel.Name = "ServerNameLabel";
            this.ServerNameLabel.Size = new System.Drawing.Size(231, 32);
            this.ServerNameLabel.TabIndex = 1;
            this.ServerNameLabel.Text = "Server Name/Adress";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(68, 100);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(132, 32);
            this.NameLabel.TabIndex = 2;
            this.NameLabel.Text = "Your Name";
            // 
            // ServerNameTextbox
            // 
            this.ServerNameTextbox.Location = new System.Drawing.Point(328, 40);
            this.ServerNameTextbox.Name = "ServerNameTextbox";
            this.ServerNameTextbox.Size = new System.Drawing.Size(416, 39);
            this.ServerNameTextbox.TabIndex = 3;
            this.ServerNameTextbox.TextChanged += new System.EventHandler(this.ServerNameTextbox_TextChanged);
            // 
            // NameTextbox
            // 
            this.NameTextbox.Location = new System.Drawing.Point(328, 100);
            this.NameTextbox.Name = "NameTextbox";
            this.NameTextbox.Size = new System.Drawing.Size(416, 39);
            this.NameTextbox.TabIndex = 4;
            this.NameTextbox.TextChanged += new System.EventHandler(this.NameTextbox_TextChanged);
            // 
            // RetrieveParticipantsButton
            // 
            this.RetrieveParticipantsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.RetrieveParticipantsButton.Location = new System.Drawing.Point(1022, 535);
            this.RetrieveParticipantsButton.Name = "RetrieveParticipantsButton";
            this.RetrieveParticipantsButton.Size = new System.Drawing.Size(464, 46);
            this.RetrieveParticipantsButton.TabIndex = 6;
            this.RetrieveParticipantsButton.Text = "Retrieve Participants";
            this.RetrieveParticipantsButton.UseVisualStyleBackColor = true;
            this.RetrieveParticipantsButton.Click += new System.EventHandler(this.RetrieveParticipantsButton_Click);
            // 
            // TypeMessageLabel
            // 
            this.TypeMessageLabel.AutoSize = true;
            this.TypeMessageLabel.Location = new System.Drawing.Point(68, 448);
            this.TypeMessageLabel.Name = "TypeMessageLabel";
            this.TypeMessageLabel.Size = new System.Drawing.Size(191, 32);
            this.TypeMessageLabel.TabIndex = 7;
            this.TypeMessageLabel.Text = "Type a message!";
            // 
            // TypeMessageTexbox
            // 
            this.TypeMessageTexbox.Location = new System.Drawing.Point(316, 436);
            this.TypeMessageTexbox.Name = "TypeMessageTexbox";
            this.TypeMessageTexbox.Size = new System.Drawing.Size(642, 110);
            this.TypeMessageTexbox.TabIndex = 8;
            this.TypeMessageTexbox.Text = "";
            this.TypeMessageTexbox.TextChanged += new System.EventHandler(this.TypeMessageTexbox_TextChanged);
            // 
            // ParticipantsTextBox
            // 
            this.ParticipantsTextBox.FormattingEnabled = true;
            this.ParticipantsTextBox.ItemHeight = 32;
            this.ParticipantsTextBox.Location = new System.Drawing.Point(1022, 36);
            this.ParticipantsTextBox.Name = "ParticipantsTextBox";
            this.ParticipantsTextBox.Size = new System.Drawing.Size(464, 484);
            this.ParticipantsTextBox.TabIndex = 10;
            // 
            // MessageDisplayTextBox
            // 
            this.MessageDisplayTextBox.FormattingEnabled = true;
            this.MessageDisplayTextBox.ItemHeight = 32;
            this.MessageDisplayTextBox.Location = new System.Drawing.Point(68, 643);
            this.MessageDisplayTextBox.Name = "MessageDisplayTextBox";
            this.MessageDisplayTextBox.Size = new System.Drawing.Size(1418, 292);
            this.MessageDisplayTextBox.TabIndex = 11;
            // 
            // connectedToServerLabel
            // 
            this.connectedToServerLabel.AutoSize = true;
            this.connectedToServerLabel.Location = new System.Drawing.Point(328, 336);
            this.connectedToServerLabel.Name = "connectedToServerLabel";
            this.connectedToServerLabel.Size = new System.Drawing.Size(240, 32);
            this.connectedToServerLabel.TabIndex = 12;
            this.connectedToServerLabel.Text = "Connected to Server!";
            this.connectedToServerLabel.Click += new System.EventHandler(this.connectedToServerLabel_Click);
            // 
            // sendMessageButton
            // 
            this.sendMessageButton.Location = new System.Drawing.Point(316, 574);
            this.sendMessageButton.Name = "sendMessageButton";
            this.sendMessageButton.Size = new System.Drawing.Size(642, 46);
            this.sendMessageButton.TabIndex = 13;
            this.sendMessageButton.Text = "Send Message";
            this.sendMessageButton.UseVisualStyleBackColor = true;
            this.sendMessageButton.Click += new System.EventHandler(this.sendMessageButton_Click);
            // 
            // ChatClientGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1548, 958);
            this.Controls.Add(this.sendMessageButton);
            this.Controls.Add(this.connectedToServerLabel);
            this.Controls.Add(this.MessageDisplayTextBox);
            this.Controls.Add(this.ParticipantsTextBox);
            this.Controls.Add(this.TypeMessageTexbox);
            this.Controls.Add(this.TypeMessageLabel);
            this.Controls.Add(this.RetrieveParticipantsButton);
            this.Controls.Add(this.NameTextbox);
            this.Controls.Add(this.ServerNameTextbox);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ServerNameLabel);
            this.Controls.Add(this.ConnectServerButton);
            this.Name = "ChatClientGUI";
            this.Text = "ChatClient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingX);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button ConnectServerButton;
        private Label ServerNameLabel;
        private Label NameLabel;
        private TextBox ServerNameTextbox;
        private TextBox NameTextbox;
        private Button RetrieveParticipantsButton;
        private Label TypeMessageLabel;
        private RichTextBox TypeMessageTexbox;
        private ListBox ParticipantsTextBox;
        private ListBox MessageDisplayTextBox;
        private Label connectedToServerLabel;
        private Button sendMessageButton;
    }
}