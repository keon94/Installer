using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;


namespace GenericInstaller
{
    public partial class InstallProcessor
    {

        private class DirectoryFields
        {
            public List<string> AbsoluteSourceDirectories; //the list of the absolute addresses of the files or folders to be transferred
            public string AbsoluteDestinationFolderDirectory; //the directory to which the AbsoluteSourceDirectories shall be written
            public int NumberOfFilesTransferred;
            public DirectoryFields(string AbsoluteDestinationFileDirectory)
            {
                this.AbsoluteSourceDirectories = new List<string>();
                this.AbsoluteDestinationFolderDirectory = AbsoluteDestinationFileDirectory;
                this.NumberOfFilesTransferred = 0;
            }
        };

        private int TotalOperations = 0;
        private string status;
        private float percentage;
        private NameValueCollection InstallationSettings;
        private bool MakeBackup;
        private string AbsoluteTargetBaseDirectory;
        private string AbsoluteTargetBackupDirectory;
        private ProgressForm progressForm;
        private List<DirectoryFields> CopyFileDirectoriesList;
        private List<DirectoryFields> CopyFolderDirectoriesList;

        public InstallProcessor(ProgressForm _progressForm)
        {
            progressForm = _progressForm;
            status = "";
            AbsoluteTargetBaseDirectory = InstallerForm.AbsoluteTargetDirectory;
            InstallationSettings = ConfigurationManager.GetSection("InstallationSettings") as NameValueCollection;
            percentage = 0;
            AbsoluteTargetBackupDirectory = InstallerForm.AbsoluteBackupDirectory;
            MakeBackup = Directory.Exists(AbsoluteTargetBackupDirectory);
            if (MakeBackup)
            {
                try
                {
                    PermissionManager.CheckWritableDirectoryPermission(AbsoluteTargetBackupDirectory);
                }
                catch (Exception)
                {
                    throw;
                }
                Directory.CreateDirectory(AbsoluteTargetBackupDirectory);
            }
        }

        public void beginInstallation() //-i: install -r: rollback
        {

            try
            {
                DirectoryCopier.installProcessor = this;
                DirectoryCopier.CopySetup("Copy Files");
                DirectoryCopier.CopySetup("Copy Folders");
                DirectoryCopier.TransferFiles();
                DirectoryCopier.TransferFolders();
            }
            catch (ThreadAbortException) //RollBack Logic
            {
                RollBack();
                Thread.ResetAbort();
            }
            catch (Exception e)
            {
                RollBack();
                progressForm.catch_ThreadException(ref e);
                Thread.CurrentThread.Abort();
            }

        }

        private void RollBack()
        {
            for (; percentage >= 0; percentage -= 100 / 10)
            {
                Thread.Sleep(500);
                status = "Rolling Back...";
                progressForm.update_progressbar(status, (int)percentage);
            }
        }

        private bool SectionExists(string section)
        {
            return this.InstallationSettings[section] != null;
        }

        


    }
}      
      

        


