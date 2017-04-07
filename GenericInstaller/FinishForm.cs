using System;
using System.Windows.Forms;

namespace GenericInstaller
{
    public partial class FinishForm : InstallerForm
    {
        public FinishForm()
        {
            InitializeComponent();
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }
    }
}
