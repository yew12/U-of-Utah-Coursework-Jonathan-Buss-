namespace GUI
{
    partial class SpreadsheetGUI
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.spreadsheetGridWidget1 = new SpreadsheetGrid_Core.SpreadsheetGridWidget();
            this.windowsOpenDisplay = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spreadsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellLocation_TextBox = new System.Windows.Forms.TextBox();
            this.contents_TextBox = new System.Windows.Forms.TextBox();
            this.value_TextBox = new System.Windows.Forms.TextBox();
            this.compute_Button = new System.Windows.Forms.Button();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetGridWidget1
            // 
            this.spreadsheetGridWidget1.BackColor = System.Drawing.Color.MediumPurple;
            this.spreadsheetGridWidget1.Location = new System.Drawing.Point(1, 134);
            this.spreadsheetGridWidget1.Name = "spreadsheetGridWidget1";
            this.spreadsheetGridWidget1.Size = new System.Drawing.Size(1688, 1012);
            this.spreadsheetGridWidget1.TabIndex = 0;
            // 
            // windowsOpenDisplay
            // 
            this.windowsOpenDisplay.Enabled = false;
            this.windowsOpenDisplay.Location = new System.Drawing.Point(1377, 89);
            this.windowsOpenDisplay.Name = "windowsOpenDisplay";
            this.windowsOpenDisplay.Size = new System.Drawing.Size(230, 39);
            this.windowsOpenDisplay.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.openToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1696, 40);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(71, 36);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spreadsheetToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(416, 44);
            this.newToolStripMenuItem.Text = "New";
            // 
            // spreadsheetToolStripMenuItem
            // 
            this.spreadsheetToolStripMenuItem.Name = "spreadsheetToolStripMenuItem";
            this.spreadsheetToolStripMenuItem.Size = new System.Drawing.Size(436, 44);
            this.spreadsheetToolStripMenuItem.Text = "New Spreadsheet (Ctrl + n)";
            this.spreadsheetToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(416, 44);
            this.openToolStripMenuItem1.Text = "Open (Ctrl + O)";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(416, 44);
            this.saveToolStripMenuItem.Text = "Save (Ctrl + S)";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(416, 44);
            this.closeToolStripMenuItem.Text = "Close (Ctrl + W/ Ctrl + Q)";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(84, 36);
            this.openToolStripMenuItem.Text = "Help";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // cellLocation_TextBox
            // 
            this.cellLocation_TextBox.Enabled = false;
            this.cellLocation_TextBox.Location = new System.Drawing.Point(1, 89);
            this.cellLocation_TextBox.Name = "cellLocation_TextBox";
            this.cellLocation_TextBox.Size = new System.Drawing.Size(86, 39);
            this.cellLocation_TextBox.TabIndex = 1;
            // 
            // contents_TextBox
            // 
            this.contents_TextBox.Location = new System.Drawing.Point(245, 89);
            this.contents_TextBox.Name = "contents_TextBox";
            this.contents_TextBox.Size = new System.Drawing.Size(872, 39);
            this.contents_TextBox.TabIndex = 2;
            this.contents_TextBox.TextChanged += new System.EventHandler(this.contents_TextBox_TextChanged);
            // 
            // value_TextBox
            // 
            this.value_TextBox.Enabled = false;
            this.value_TextBox.Location = new System.Drawing.Point(93, 89);
            this.value_TextBox.Name = "value_TextBox";
            this.value_TextBox.Size = new System.Drawing.Size(132, 39);
            this.value_TextBox.TabIndex = 3;
            this.value_TextBox.TextChanged += new System.EventHandler(this.value_TextBox_TextChanged);
            // 
            // compute_Button
            // 
            this.compute_Button.Location = new System.Drawing.Point(1123, 85);
            this.compute_Button.Name = "compute_Button";
            this.compute_Button.Size = new System.Drawing.Size(222, 46);
            this.compute_Button.TabIndex = 6;
            this.compute_Button.Text = "Set cell contents";
            this.compute_Button.UseVisualStyleBackColor = true;
            this.compute_Button.Click += new System.EventHandler(this.compute_Button_Click);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            // 
            // SpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1696, 1162);
            this.Controls.Add(this.compute_Button);
            this.Controls.Add(this.windowsOpenDisplay);
            this.Controls.Add(this.value_TextBox);
            this.Controls.Add(this.contents_TextBox);
            this.Controls.Add(this.cellLocation_TextBox);
            this.Controls.Add(this.spreadsheetGridWidget1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpreadsheetGUI";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosingCloserCheck);
            this.Load += new System.EventHandler(this.SpreadsheetGUI_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private SpreadsheetGrid_Core.SpreadsheetGridWidget spreadsheetGridWidget1;

        private TextBox windowsOpenDisplay;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem spreadsheetToolStripMenuItem;

        private TextBox cellLocation_TextBox;
        private TextBox contents_TextBox;
        private TextBox value_TextBox;
        private Button compute_Button;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
    }
}