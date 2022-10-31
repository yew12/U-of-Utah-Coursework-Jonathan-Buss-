namespace TowardAgarioStepOne
{
    partial class TowardAgarioStepOne
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
            this.PrintCenterofCircle = new System.Windows.Forms.Label();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // PrintCenterofCircle
            // 
            this.PrintCenterofCircle.AutoSize = true;
            this.PrintCenterofCircle.Location = new System.Drawing.Point(550, 50);
            this.PrintCenterofCircle.Name = "PrintCenterofCircle";
            this.PrintCenterofCircle.Size = new System.Drawing.Size(56, 32);
            this.PrintCenterofCircle.TabIndex = 0;
            this.PrintCenterofCircle.Text = "PCC";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // TowardAgarioStepOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 629);
            this.Controls.Add(this.PrintCenterofCircle);
            this.Name = "TowardAgarioStepOne";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label PrintCenterofCircle;
        private FileSystemWatcher fileSystemWatcher1;
    }
}