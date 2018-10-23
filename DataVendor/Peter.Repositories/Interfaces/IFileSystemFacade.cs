namespace Peter.Repositories.Interfaces
{
    interface IFileSystemFacade
    {
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
        /// Saves the content into a text file.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="content"></param>
        void Save(string fullPath, string content);
    }
}
