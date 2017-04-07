using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;

namespace GenericInstaller
{



    public abstract class InstallerForm : Form
    {

        protected static string InstallingApplicationName;
        protected static string TargetApplicationName;
        protected static string LicenseInfo;
        protected static string CustomMessage;
        protected static bool MakeBackup;
        protected static string BackupMessage;

        protected bool PromptUserOnExit;

        public static string AbsoluteTargetDirectory { get; protected set; }
        public static string AbsoluteBackupDirectory { get; protected set; }

        public static NameValueCollection InterfaceSettings { get; protected set; }

        public static List<Form> forms { get; protected set; }

        public InstallerForm() 
        {             
            this.PromptUserOnExit = true;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 390);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        
        protected void InstallerFormInitialise()
        {
            try
            {
                
                InstallerForm.forms = new List<Form>();
                InterfaceSettings = ConfigurationManager.GetSection("InterfaceSettings") as NameValueCollection;
                InstallingApplicationName = InterfaceSettings["Installing Application Name"];
                TargetApplicationName = InterfaceSettings["Target Application Name"];
                AbsoluteTargetDirectory = InterfaceSettings["Target Application Default Directory"];
                LicenseInfo = InterfaceSettings["License Information"];
                AbsoluteBackupDirectory = InterfaceSettings["Backup Default Directory"];
            }
            catch (ConfigurationErrorsException)
            {
                if (InterfaceSettings == null)
                {
                    DialogResult dialog = MessageBox.Show("InterfaceSettings Section Missing in the config file. Quitting.", "Error", MessageBoxButtons.OK);
                    System.Environment.Exit(0); 
                }
                else if (InstallingApplicationName == null)
                {
                    InstallingApplicationName = "Unknown Application.";
                }
                else if (TargetApplicationName == null)
                {
                    TargetApplicationName = "this computer.";
                }
                else if (AbsoluteTargetDirectory == null)
                {
                    Environment.GetEnvironmentVariable("PROGRAMFILES");
                }
                else if (LicenseInfo == null)
                {
                    LicenseInfo = "License Information: None.";
                }
            }
            
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (this.PromptUserOnExit)
                {
                    DialogResult dialog = MessageBox.Show(this, "Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo);
                    if (dialog != DialogResult.Yes)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                Application.Exit();
            }
        }

    }
}