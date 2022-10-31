namespace ClientGUI
{
    partial class ClientGUI
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
            this.playerNameLabel = new System.Windows.Forms.Label();
            this.serverLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.hpsLabel = new System.Windows.Forms.Label();
            this.foodLabel = new System.Windows.Forms.Label();
            this.massLabel = new System.Windows.Forms.Label();
            this.widthLabel = new System.Windows.Forms.Label();
            this.positionLabel = new System.Windows.Forms.Label();
            this.errorMessageLabel = new System.Windows.Forms.Label();
            this.hpsActualLabel = new System.Windows.Forms.Label();
            this.foodActualLabel = new System.Windows.Forms.Label();
            this.massActualLabel = new System.Windows.Forms.Label();
            this.widthActualLabel = new System.Windows.Forms.Label();
            this.positionActualLabel = new System.Windows.Forms.Label();
            this.connectServerButton = new System.Windows.Forms.Button();
            this.coordinatesLabel = new System.Windows.Forms.Label();
            this.CoordinateLabel = new System.Windows.Forms.Label();
            this.CoordinateText = new System.Windows.Forms.Label();
            this.MouseWorldCoordinate = new System.Windows.Forms.Label();
            this.worldCoordinateText = new System.Windows.Forms.Label();
            this.playerDeadLabel = new System.Windows.Forms.Label();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // playerNameLabel
            // 
            this.playerNameLabel.AutoSize = true;
            this.playerNameLabel.Location = new System.Drawing.Point(660, 264);
            this.playerNameLabel.Name = "playerNameLabel";
            this.playerNameLabel.Size = new System.Drawing.Size(149, 32);
            this.playerNameLabel.TabIndex = 0;
            this.playerNameLabel.Text = "Player Name";
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(728, 403);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(81, 32);
            this.serverLabel.TabIndex = 1;
            this.serverLabel.Text = "Server";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(867, 264);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(200, 39);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(867, 403);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(200, 39);
            this.serverTextBox.TabIndex = 3;
            this.serverTextBox.TextChanged += new System.EventHandler(this.serverTextBox_TextChanged);
            // 
            // hpsLabel
            // 
            this.hpsLabel.AutoSize = true;
            this.hpsLabel.Location = new System.Drawing.Point(1213, 82);
            this.hpsLabel.Name = "hpsLabel";
            this.hpsLabel.Size = new System.Drawing.Size(0, 32);
            this.hpsLabel.TabIndex = 5;
            // 
            // foodLabel
            // 
            this.foodLabel.AutoSize = true;
            this.foodLabel.Location = new System.Drawing.Point(1213, 267);
            this.foodLabel.Name = "foodLabel";
            this.foodLabel.Size = new System.Drawing.Size(0, 32);
            this.foodLabel.TabIndex = 7;
            // 
            // massLabel
            // 
            this.massLabel.AutoSize = true;
            this.massLabel.Location = new System.Drawing.Point(1213, 316);
            this.massLabel.Name = "massLabel";
            this.massLabel.Size = new System.Drawing.Size(0, 32);
            this.massLabel.TabIndex = 8;
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(1213, 368);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(0, 32);
            this.widthLabel.TabIndex = 9;
            // 
            // positionLabel
            // 
            this.positionLabel.AutoSize = true;
            this.positionLabel.Location = new System.Drawing.Point(1213, 422);
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(0, 32);
            this.positionLabel.TabIndex = 10;
            this.positionLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // errorMessageLabel
            // 
            this.errorMessageLabel.AutoSize = true;
            this.errorMessageLabel.BackColor = System.Drawing.SystemColors.HotTrack;
            this.errorMessageLabel.Location = new System.Drawing.Point(65, 818);
            this.errorMessageLabel.Name = "errorMessageLabel";
            this.errorMessageLabel.Size = new System.Drawing.Size(0, 32);
            this.errorMessageLabel.TabIndex = 11;
            // 
            // hpsActualLabel
            // 
            this.hpsActualLabel.AutoSize = true;
            this.hpsActualLabel.Location = new System.Drawing.Point(1394, 82);
            this.hpsActualLabel.Name = "hpsActualLabel";
            this.hpsActualLabel.Size = new System.Drawing.Size(0, 32);
            this.hpsActualLabel.TabIndex = 13;
            // 
            // foodActualLabel
            // 
            this.foodActualLabel.AutoSize = true;
            this.foodActualLabel.Location = new System.Drawing.Point(1432, 267);
            this.foodActualLabel.Name = "foodActualLabel";
            this.foodActualLabel.Size = new System.Drawing.Size(0, 32);
            this.foodActualLabel.TabIndex = 15;
            // 
            // massActualLabel
            // 
            this.massActualLabel.AutoSize = true;
            this.massActualLabel.Location = new System.Drawing.Point(1432, 316);
            this.massActualLabel.Name = "massActualLabel";
            this.massActualLabel.Size = new System.Drawing.Size(0, 32);
            this.massActualLabel.TabIndex = 16;
            // 
            // widthActualLabel
            // 
            this.widthActualLabel.AutoSize = true;
            this.widthActualLabel.Location = new System.Drawing.Point(1432, 368);
            this.widthActualLabel.Name = "widthActualLabel";
            this.widthActualLabel.Size = new System.Drawing.Size(0, 32);
            this.widthActualLabel.TabIndex = 17;
            // 
            // positionActualLabel
            // 
            this.positionActualLabel.AutoSize = true;
            this.positionActualLabel.Location = new System.Drawing.Point(1432, 422);
            this.positionActualLabel.Name = "positionActualLabel";
            this.positionActualLabel.Size = new System.Drawing.Size(0, 32);
            this.positionActualLabel.TabIndex = 18;
            // 
            // connectServerButton
            // 
            this.connectServerButton.BackColor = System.Drawing.Color.Crimson;
            this.connectServerButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.connectServerButton.Location = new System.Drawing.Point(803, 484);
            this.connectServerButton.Name = "connectServerButton";
            this.connectServerButton.Size = new System.Drawing.Size(340, 236);
            this.connectServerButton.TabIndex = 19;
            this.connectServerButton.Text = "Connect";
            this.connectServerButton.UseVisualStyleBackColor = false;
            this.connectServerButton.Click += new System.EventHandler(this.connectServerButton_Click);
            // 
            // coordinatesLabel
            // 
            this.coordinatesLabel.AutoSize = true;
            this.coordinatesLabel.Location = new System.Drawing.Point(65, 850);
            this.coordinatesLabel.Name = "coordinatesLabel";
            this.coordinatesLabel.Size = new System.Drawing.Size(138, 32);
            this.coordinatesLabel.TabIndex = 20;
            this.coordinatesLabel.Text = "coordinates";
            // 
            // CoordinateLabel
            // 
            this.CoordinateLabel.AutoSize = true;
            this.CoordinateLabel.Location = new System.Drawing.Point(816, 82);
            this.CoordinateLabel.Name = "CoordinateLabel";
            this.CoordinateLabel.Size = new System.Drawing.Size(301, 32);
            this.CoordinateLabel.TabIndex = 21;
            this.CoordinateLabel.Text = "Mouse Screen Coordinates";
            // 
            // CoordinateText
            // 
            this.CoordinateText.AutoSize = true;
            this.CoordinateText.Location = new System.Drawing.Point(1232, 82);
            this.CoordinateText.Name = "CoordinateText";
            this.CoordinateText.Size = new System.Drawing.Size(60, 32);
            this.CoordinateText.TabIndex = 22;
            this.CoordinateText.Text = "[X,Y]";
            // 
            // MouseWorldCoordinate
            // 
            this.MouseWorldCoordinate.AutoSize = true;
            this.MouseWorldCoordinate.Location = new System.Drawing.Point(1213, 546);
            this.MouseWorldCoordinate.Name = "MouseWorldCoordinate";
            this.MouseWorldCoordinate.Size = new System.Drawing.Size(0, 32);
            this.MouseWorldCoordinate.TabIndex = 23;
            // 
            // worldCoordinateText
            // 
            this.worldCoordinateText.AutoSize = true;
            this.worldCoordinateText.Location = new System.Drawing.Point(1525, 546);
            this.worldCoordinateText.Name = "worldCoordinateText";
            this.worldCoordinateText.Size = new System.Drawing.Size(0, 32);
            this.worldCoordinateText.TabIndex = 24;
            // 
            // playerDeadLabel
            // 
            this.playerDeadLabel.AutoSize = true;
            this.playerDeadLabel.Location = new System.Drawing.Point(887, 449);
            this.playerDeadLabel.Name = "playerDeadLabel";
            this.playerDeadLabel.Size = new System.Drawing.Size(145, 32);
            this.playerDeadLabel.TabIndex = 25;
            this.playerDeadLabel.Text = "Player dead!";
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.AutoSize = true;
            this.instructionsLabel.Location = new System.Drawing.Point(757, 783);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(65, 32);
            this.instructionsLabel.TabIndex = 26;
            this.instructionsLabel.Text = "label";
            // 
            // ClientGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1672, 896);
            this.Controls.Add(this.instructionsLabel);
            this.Controls.Add(this.playerDeadLabel);
            this.Controls.Add(this.worldCoordinateText);
            this.Controls.Add(this.MouseWorldCoordinate);
            this.Controls.Add(this.CoordinateText);
            this.Controls.Add(this.CoordinateLabel);
            this.Controls.Add(this.coordinatesLabel);
            this.Controls.Add(this.connectServerButton);
            this.Controls.Add(this.positionActualLabel);
            this.Controls.Add(this.widthActualLabel);
            this.Controls.Add(this.massActualLabel);
            this.Controls.Add(this.foodActualLabel);
            this.Controls.Add(this.hpsActualLabel);
            this.Controls.Add(this.errorMessageLabel);
            this.Controls.Add(this.positionLabel);
            this.Controls.Add(this.widthLabel);
            this.Controls.Add(this.massLabel);
            this.Controls.Add(this.foodLabel);
            this.Controls.Add(this.hpsLabel);
            this.Controls.Add(this.serverTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.playerNameLabel);
            this.Name = "ClientGUI";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingX);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ClientGUI_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private Label playerNameLabel;
        private Label serverLabel;
        private TextBox nameTextBox;
        private TextBox serverTextBox;
        private Label hpsLabel;
        private Label foodLabel;
        private Label massLabel;
        private Label widthLabel;
        private Label positionLabel;
        private Label errorMessageLabel;
        private Label hpsActualLabel;
        private Label foodActualLabel;
        private Label massActualLabel;
        private Label widthActualLabel;
        private Label positionActualLabel;
        private Button connectServerButton;
        private Label coordinatesLabel;
        private Label CoordinateLabel;
        private Label CoordinateText;
        private Label MouseWorldCoordinate;
        private Label worldCoordinateText;
        private Label playerDeadLabel;
        private Label instructionsLabel;
    }
}