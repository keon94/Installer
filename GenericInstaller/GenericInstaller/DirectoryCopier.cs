using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace GenericInstaller
{
    public partial class InstallProcessor
    {

        private static class DirectoryCopier
        {

            public static InstallProcessor installProcessor;



            public static void CopySetup(string CopySection)
            {
                if (!installProcessor.SectionExists(CopySection)) return;
                try
                {
                    string[] Directories = installProcessor.InstallationSettings[CopySection].Split(';');
                    string[] RelativeDirectories;
                    //correct the format of the strings (directories)
                    for (int i = 0; i < Directories.Length; ++i)
                    {
                        Directories[i] = Directories[i].Replace("\\\r", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);
                        Directories[i] = Directories[i].Trim();
                        if (!String.IsNullOrWhiteSpace(Directories[i]))
                        {
                            RelativeDirectories = Directories[i].Split(',');

                            if (CopySection == "Copy Files")
                            {
                                if (installProcessor.CopyFileDirectoriesList == null) installProcessor.CopyFileDirectoriesList = new List<DirectoryFields>();
                                installProcessor.CopyFileDirectoriesList.Add(new DirectoryFields(Path.Combine(installProcessor.AbsoluteTargetBaseDirectory, RelativeDirectories[1]))); //Src in Regex form , Dest.
                                ParseSourceFilesDirectories(RelativeDirectories[0]);
                            }
                            else if (CopySection == "Copy Folders")
                            {
                                if (installProcessor.CopyFolderDirectoriesList == null) installProcessor.CopyFolderDirectoriesList = new List<DirectoryFields>();
                                installProcessor.CopyFolderDirectoriesList.Add(new DirectoryFields(Path.Combine(installProcessor.AbsoluteTargetBaseDirectory, RelativeDirectories[1]))); //Src in Regex form , Dest.
                                ParseSourceFoldersDirectories(RelativeDirectories[0]);
                            }
                            else throw new Exception("Invalid Copy Section used in the source code.");
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    return;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private static void ParseSourceFoldersDirectories(string RelativeRegexSourceDirectory)
            {
                string AbsoluteRegexSourceFileDirectories = "";
                try
                {
                    DirectoryFields directoryFields = installProcessor.CopyFolderDirectoriesList[installProcessor.CopyFolderDirectoriesList.Count - 1];
                    AbsoluteRegexSourceFileDirectories = Path.Combine(Directory.GetCurrentDirectory(), Path.GetDirectoryName(RelativeRegexSourceDirectory));
                    string pattern = RelativeRegexSourceDirectory.Substring(RelativeRegexSourceDirectory.LastIndexOf('\\') + 1);
                    Regex RegexEngine = new Regex(pattern);
                    IEnumerable<string> AbsoluteSourceFolders = from Folders in
                                                                    Directory.EnumerateDirectories(AbsoluteRegexSourceFileDirectories, "*",
                                                                    SearchOption.TopDirectoryOnly)
                                                                where RegexEngine.IsMatch(Folders)
                                                                select Folders;
                    installProcessor.CopyFolderDirectoriesList[installProcessor.CopyFolderDirectoriesList.Count - 1].AbsoluteSourceDirectories.AddRange(AbsoluteSourceFolders);
                    installProcessor.TotalOperations += installProcessor.CopyFolderDirectoriesList[installProcessor.CopyFolderDirectoriesList.Count - 1].AbsoluteSourceDirectories.Count;
                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    throw new Exception("The path " + AbsoluteRegexSourceFileDirectories + "does not exist. Check the installer's config file and make sure the sources under the Copy Folders section have proper format, and point to correct relative locations.");
                }
                catch (Exception)
                {
                    throw;
                }

            }

            private static void ParseSourceFilesDirectories(string RelativeRegexSourceDirectory) //determines the absolute source directories (per file) and puts them in the copyDirectoriesList
            {

                DirectoryFields directoryFields = installProcessor.CopyFileDirectoriesList[installProcessor.CopyFileDirectoriesList.Count - 1];
                string AbsoluteRegexSourceFileDirectories = Path.Combine(Directory.GetCurrentDirectory(), Path.GetDirectoryName(RelativeRegexSourceDirectory));
                string pattern = RelativeRegexSourceDirectory.Substring(RelativeRegexSourceDirectory.LastIndexOf('\\') + 1);
                try
                {
                    Regex RegexEngine = new Regex(pattern);
                    directoryFields.AbsoluteSourceDirectories = Directory.GetFiles(AbsoluteRegexSourceFileDirectories, "*") //get all the files that match the regex
                                .Where(path => RegexEngine.IsMatch(path))
                                .ToList();
                }
                catch (System.IO.DirectoryNotFoundException) //basePath does not exist
                {
                    throw new Exception("The path " + AbsoluteRegexSourceFileDirectories + " does not exist. Check the installer's config file and make sure the sources under the Copy Files section have proper format, and point to the correct relative locations.");
                }
                catch (System.ArgumentException)
                {
                    throw;
                }

                installProcessor.TotalOperations += installProcessor.CopyFileDirectoriesList[installProcessor.CopyFileDirectoriesList.Count - 1].AbsoluteSourceDirectories.Count;
                installProcessor.CopyFileDirectoriesList[installProcessor.CopyFileDirectoriesList.Count - 1] = directoryFields;
            }

            public static string GetRelativePath(string AbsolutePath, string BasePath)
            {
                return AbsolutePath.Trim('\\').Replace(BasePath.Trim('\\'), "").Trim('\\');
            }

            public static void TransferFiles()
            {
                if (!installProcessor.SectionExists("Copy Files")) return;
                int FileTransferCounter;
                string RelativeDestinationDirectory;
                foreach (DirectoryFields FileDirectoryFields in installProcessor.CopyFileDirectoriesList)
                {
                    RelativeDestinationDirectory = GetRelativePath(FileDirectoryFields.AbsoluteDestinationFolderDirectory, installProcessor.AbsoluteTargetBaseDirectory);
                    Directory.CreateDirectory(FileDirectoryFields.AbsoluteDestinationFolderDirectory);
                    FileTransferCounter = 0;
                    string DestinationFilePath, AbsoluteBackupDestination;
                    foreach (string AbsoluteSourceFilePath in FileDirectoryFields.AbsoluteSourceDirectories)
                    {
                        if (installProcessor.MakeBackup)
                        {
                            DestinationFilePath = Path.Combine(FileDirectoryFields.AbsoluteDestinationFolderDirectory, System.IO.Path.GetFileName(AbsoluteSourceFilePath));
                            if (File.Exists(DestinationFilePath))
                            {
                                AbsoluteBackupDestination = Path.Combine(installProcessor.AbsoluteTargetBackupDirectory, RelativeDestinationDirectory);
                                Directory.CreateDirectory(AbsoluteBackupDestination);
                                System.IO.File.Copy(Path.Combine(FileDirectoryFields.AbsoluteDestinationFolderDirectory, System.IO.Path.GetFileName(AbsoluteSourceFilePath)), Path.Combine(AbsoluteBackupDestination, System.IO.Path.GetFileName(AbsoluteSourceFilePath)), true);
                            }
                        }
                        installProcessor.status = "Copying from: " + Path.GetFileName(AbsoluteSourceFilePath) + " to\n" + FileDirectoryFields.AbsoluteDestinationFolderDirectory;
                        System.IO.File.Copy(FileDirectoryFields.AbsoluteSourceDirectories[FileTransferCounter], Path.Combine(FileDirectoryFields.AbsoluteDestinationFolderDirectory, System.IO.Path.GetFileName(AbsoluteSourceFilePath)), true);
                        installProcessor.percentage += (float)100.0 / installProcessor.TotalOperations;
                        installProcessor.progressForm.update_progressbar(installProcessor.status, (int)installProcessor.percentage);
                        FileTransferCounter++;
                        FileDirectoryFields.NumberOfFilesTransferred = FileTransferCounter;
                    }
                }
            }

            public static void TransferFolders()
            {
                if (!installProcessor.SectionExists("Copy Folders")) return;
                int FolderTransferCounter;
                string RelativeDestinationDirectory;
                foreach (DirectoryFields FolderDirectoryFields in installProcessor.CopyFolderDirectoriesList)
                {
                    RelativeDestinationDirectory = GetRelativePath(FolderDirectoryFields.AbsoluteDestinationFolderDirectory, installProcessor.AbsoluteTargetBaseDirectory);
                    Directory.CreateDirectory(FolderDirectoryFields.AbsoluteDestinationFolderDirectory);
                    FolderTransferCounter = 0;
                    string DestinationFolderPath, AbsoluteBackupDestination;
                    foreach (string AbsoluteSourceFolderPath in FolderDirectoryFields.AbsoluteSourceDirectories)
                    {
                        if (installProcessor.MakeBackup)
                        {
                            DestinationFolderPath = Path.Combine(FolderDirectoryFields.AbsoluteDestinationFolderDirectory, System.IO.Path.GetFileName(AbsoluteSourceFolderPath));
                            if (Directory.Exists(DestinationFolderPath))
                            {
                                AbsoluteBackupDestination = Path.Combine(installProcessor.AbsoluteTargetBackupDirectory, RelativeDestinationDirectory);
                                Directory.CreateDirectory(AbsoluteBackupDestination);
                                CloneFolder(DestinationFolderPath, AbsoluteBackupDestination);
                            }
                        }
                        installProcessor.status = "Copying folder: " + Path.GetFileName(AbsoluteSourceFolderPath) + " to\n" + FolderDirectoryFields.AbsoluteDestinationFolderDirectory;
                        CloneFolder(AbsoluteSourceFolderPath, FolderDirectoryFields.AbsoluteDestinationFolderDirectory);
                        installProcessor.percentage += (float)100.0 / installProcessor.TotalOperations;
                        installProcessor.progressForm.update_progressbar(installProcessor.status, (int)installProcessor.percentage);
                        FolderTransferCounter++;
                        FolderDirectoryFields.NumberOfFilesTransferred = FolderTransferCounter;
                    }
                }
            }

            public static void CloneFolder(string AbsoluteSourceFolder, string AbsoluteDestinationFolder)
            {

                try
                {
                    if (!Directory.Exists(Path.Combine(AbsoluteDestinationFolder, Path.GetFileName(AbsoluteSourceFolder))))
                    {
                        Directory.CreateDirectory(Path.Combine(AbsoluteDestinationFolder, Path.GetFileName(AbsoluteSourceFolder)));
                    }
                    CloneFolderContentsRecursively(AbsoluteSourceFolder, Path.Combine(AbsoluteDestinationFolder, Path.GetFileName(AbsoluteSourceFolder)));
                }
                catch (Exception)
                {
                    throw;
                }


            }

            private static void CloneFolderContentsRecursively(string AbsoluteSourceFolder, string AbsoluteDestinationFolder)
            {
                try
                {

                    foreach (var directory in Directory.GetDirectories(AbsoluteSourceFolder, "*", SearchOption.TopDirectoryOnly))
                    {
                        string dirName = Path.GetFileName(directory);
                        if (!Directory.Exists(Path.Combine(AbsoluteDestinationFolder, dirName)))
                        {
                            Directory.CreateDirectory(Path.Combine(AbsoluteDestinationFolder, dirName));
                        }
                        CloneFolderContentsRecursively(directory, Path.Combine(AbsoluteDestinationFolder, dirName));
                    }

                    foreach (var file in Directory.GetFiles(AbsoluteSourceFolder))
                    {
                        File.Copy(file, Path.Combine(AbsoluteDestinationFolder, Path.GetFileName(file)), true);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        
        
        
        
        }

    }
}
