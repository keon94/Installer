


namespace GenericInstaller
{
    partial class ProgressForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.CancelButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.InstallLabel = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(400, 352);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 0;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(87, 254);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(327, 23);
            this.progressBar.TabIndex = 1;
            // 
            // InstallLabel
            // 
            this.InstallLabel.AutoSize = true;
            this.InstallLabel.Location = new System.Drawing.Point(87, 225);
            this.InstallLabel.Name = "InstallLabel";
            this.InstallLabel.Size = new System.Drawing.Size(51, 13);
            this.InstallLabel.TabIndex = 2;
            this.InstallLabel.Text = "Installing:";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // 
            // ProgressForm
            // 
            this.Controls.Add(this.InstallLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.CancelButton);
            this.Name = "ProgressForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label InstallLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}