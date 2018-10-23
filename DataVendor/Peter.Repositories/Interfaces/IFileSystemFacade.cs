namespace Peter.Repositories.Interfaces
{
    interface IFileSystemFacade
    {
        /// <summary>
        /// Tries to back up a file (into another folder and/or under another name).
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="backupFullPath"></param>
        /// <param name="message">Error message (if any).</param>
        /// <returns>True if operation was successful.</returns>
        bool TryBackup(string fullPath, string backupFullPath, out string message);

        /// <summary>
        /// Tries to load a file content.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="content"></param>
        /// <param name="message">Error message (if any).</param>
        /// <returns>True if operation was successful.</returns>
        bool TryLoad(string fullPath, out string content, out string message);

        /// <summary>
        /// Tries to save a file content.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="content"></param>
        /// <param name="message">Error message (if any).</param>
        /// <returns>True if operation was successful.</returns>
        bool TrySave(string fullPath, string content, out string message);
    }
}
