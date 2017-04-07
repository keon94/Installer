using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;

namespace GenericInstaller
{
    class PermissionManager
    {
        internal static void CheckWritableDirectoryPermission(string path)
        {
            bool writeAllow = false;
            bool writeDeny = false;
            DirectorySecurity accessControlList;
            try
            {
                Directory.CreateDirectory(path);
                accessControlList = Directory.GetAccessControl(path);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }

            AuthorizationRuleCollection accessRules = accessControlList.GetAccessRules(true, true,
                                        typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
            {
                //MessageBox.Show("Could not get directory access rules.", "Error", MessageBoxButtons.OK);
                throw new Exception("Could not get directory access rules.");
            }

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                    continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    writeAllow = true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    writeDeny = true;
            }

            bool DirectoryIsWritable = writeAllow && !writeDeny;
            if (!DirectoryIsWritable)
            {
                //MessageBox.Show("Can't install to " + path + ". Permission is denied.", "Error", MessageBoxButtons.OK);
                throw new Exception("Can't write to " + path + ". Permission is denied.");
            }
        }
    }
}
