using System;
using System.IO;
using Peter.Repositories.Interfaces;

namespace Peter.Repositories.Implementations
{
    internal class FileSystemFacade : IFileSystemFacade
    {
        public bool TryBackup(string fullPath, string backupFullPath, out string message)
        {
            message = string.Empty;

            try
            {
                var backupDir = Path.GetDirectoryName(backupFullPath);
                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }

                File.Move(fullPath, backupFullPath);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public bool TryLoad(string fullPath, out string content, out string message)
        {
            content = string.Empty;
            message = string.Empty;

            try
            {
                content = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public bool TrySave(string fullPath, string content, out string message)
        {
            message = string.Empty;

            try
            {
                var dir = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(fullPath, content);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}
