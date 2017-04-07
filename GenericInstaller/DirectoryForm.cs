using System;
using System.Windows.Forms;

namespace GenericInstaller
{
    internal partial class DirectoryForm : InstallerForm
    {

        public DirectoryForm()
        {
            InitializeComponent();
            forms.Add(this);
            DirectoryTextBox.Text = InstallerForm.AbsoluteTargetDirectory;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            InstallerForm.AbsoluteTargetDirectory = DirectoryTextBox.Text;
            try
            {
                PermissionManager.CheckWritableDirectoryPermission(InstallerForm.AbsoluteTargetDirectory);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK);
                return;
            }
            
            this.Hide();
            Form SummaryForm;
            if (InstallerForm.forms.Count == 2)
            {
                SummaryForm = new SummaryForm();
            }
            else
            {
                SummaryForm = InstallerForm.forms[2];
            }
            SummaryForm.Show();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form StartingForm = InstallerForm.forms[0];
            StartingForm.Show();
        }

        private void CustomizeButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DirectoryTextBox.Text = folderBrowserDialog1.SelectedPath;               
            }
        }


    




    }
}
