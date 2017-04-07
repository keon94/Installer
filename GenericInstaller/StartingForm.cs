using System;
using System.Windows.Forms;

namespace GenericInstaller
{
    public partial class StartingForm : InstallerForm
    {


        public StartingForm()
        {
            InstallerFormInitialise();
            InitializeComponent();
            InstallerForm.forms.Add(this);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form DirectoryForm;
            if (InstallerForm.forms.Count == 1)
            {
                DirectoryForm = new DirectoryForm();
            }
            else
            {
                DirectoryForm = (DirectoryForm)InstallerForm.forms[1];
            }
            DirectoryForm.Show();
            
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
