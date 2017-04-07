namespace GenericInstaller
{
    partial class DirectoryForm
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
            this.DirectoryTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NextButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.CustomizeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.DirectoryTextBox.Location = new System.Drawing.Point(48, 194);
            this.DirectoryTextBox.Name = "textBox1";
            this.DirectoryTextBox.Size = new System.Drawing.Size(326, 20);
            this.DirectoryTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Installation Directory:";
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(389, 352);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 2;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // BackButton
            // 
            this.BackButton.Location = new System.Drawing.Point(299, 352);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 23);
            this.BackButton.TabIndex = 3;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // CustomizeButton
            // 
            this.CustomizeButton.Location = new System.Drawing.Point(389, 191);
            this.CustomizeButton.Name = "CustomizeButton";
            this.CustomizeButton.Size = new System.Drawing.Size(75, 23);
            this.CustomizeButton.TabIndex = 4;
            this.CustomizeButton.Text = "Customize";
            this.CustomizeButton.UseVisualStyleBackColor = true;
            this.CustomizeButton.Click += new System.EventHandler(this.CustomizeButton_Click);
            // 
            // DirectoryForm
            // 
            this.Controls.Add(this.CustomizeButton);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DirectoryTextBox);
            this.Name = "InstallerWindow";
            this.Text = InstallerForm.InstallingApplicationName + " Installer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DirectoryTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button CustomizeButton;
    }
}