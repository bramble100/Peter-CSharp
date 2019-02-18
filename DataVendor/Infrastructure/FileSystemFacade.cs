using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;

namespace Infrastructure
{
    public class FileSystemFacade : IFileSystemFacade
    {
        private readonly Logger _logger;

        public FileSystemFacade()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public string DesktopFolder => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public void Backup(string fullPath, string backupFullPath)
        {
            try
            {
                var dirName = Path.GetDirectoryName(backupFullPath);

                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }

                File.Move(fullPath, backupFullPath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Backup directory cannot be created. {ex.Message}");
                throw new FileSystemFacadeException($"Backup directory cannot be created. {ex.Message}", ex);
            }
        }

        public string Load(string fullPath)
        {
            try
            {
                return File.ReadAllText(fullPath, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"File cannot be loaded. {ex.Message}");
                throw new FileSystemFacadeException($"File cannot be loaded. {ex.Message}", ex);
            }
        }

        public StreamReader Open(string fullPath) => File.OpenText(fullPath);

        public IEnumerable<string> ReadLines(string fullPath, Encoding encoding)
        {
            try
            {
                return File.ReadLines(fullPath, encoding);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"File content cannot be read. {ex.Message}");
                throw new FileSystemFacadeException($"File content cannot be read. {ex.Message}", ex);
            }
        }

        public void Save(string fullPath, string content)
        {
            try
            {
                var dirName = Path.GetDirectoryName(fullPath);

                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }

                File.WriteAllText(fullPath, content, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"File cannot be saved. {ex.Message}");
                throw new FileSystemFacadeException($"File cannot be saved. {ex.Message}", ex);
            }
        }
    }
}
