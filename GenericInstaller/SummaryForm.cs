using System;
using System.Windows.Forms;

namespace GenericInstaller
{
    internal partial class SummaryForm : InstallerForm
    {
        public SummaryForm()
        {
            InitializeComponent();
            forms.Add(this);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {

            PromptBackup();         
            this.Hide();
            InstallerForm.forms.Clear();
            Form ProgressForm = new ProgressForm();
            ProgressForm.Show();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            DirectoryForm DirectoryForm = (DirectoryForm)InstallerForm.forms[1];
            DirectoryForm.Show();
        }

        private void PromptBackup()
        {
            DialogResult dialog = MessageBox.Show("Create Backup?", "Backup", MessageBoxButtons.YesNo);                                 
            if (dialog == DialogResult.Yes) {
                dialog = MessageBox.Show("Choose Your Backup Directory in the next window.", "", MessageBoxButtons.OK);
                if (dialog == DialogResult.OK)
                {
                    using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                    {
                        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                        {
                            InstallerForm.AbsoluteBackupDirectory = folderBrowserDialog.SelectedPath;
                        }
                    }
                }                
            
            }
        }
    }
}
