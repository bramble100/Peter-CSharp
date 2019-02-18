using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;

namespace Infrastructure
{
    /// <summary>
    /// Provides access to the file system.
    /// </summary>
    public class FileSystemFacade : IFileSystemFacade
    {
        private readonly Logger _logger;

        /// <summary>
        /// Returns the user's desktop folder.
        /// </summary>
        public string DesktopFolder => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileSystemFacade()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Backs up a file (into another folder and/or under another name).
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="backupFullPath"></param>
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
        /// <summary>
        /// Loads the content of a text file.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Opens an existing UTF-8 encoded text file for reading.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public StreamReader Open(string fullPath) => File.OpenText(fullPath);
        /// <summary>
        /// Read the lines of a file that has a specified encoding.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Saves the content into a text file.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="content"></param>
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
