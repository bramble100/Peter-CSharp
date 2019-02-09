using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Peter.Repositories.Interfaces
{
    /// <summary>
    /// Provides access to the file system.
    /// </summary>
    public interface IFileSystemFacade
    {
        /// <summary>
        /// Returns the user's desktop folder.
        /// </summary>
        string DesktopFolder { get; }

        /// <summary>
        /// Backs up a file (into another folder and/or under another name).
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="backupFullPath"></param>
        void Backup(string fullPath, string backupFullPath);
        /// <summary>
        /// Loads the content of a text file.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        string Load(string fullPath);
        /// <summary>
        /// Opens an existing UTF-8 encoded text file for reading.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        StreamReader Open(string fullPath);
        /// <summary>
        /// Read the lines of a file that has a specified encoding.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="uTF8"></param>
        /// <returns></returns>
        IEnumerable<string> ReadLines(string fullPath, Encoding uTF8);
        /// <summary>
        /// Saves the content into a text file.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="content"></param>
        void Save(string fullPath, string content);
    }
}
