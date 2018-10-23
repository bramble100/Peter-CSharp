using System.IO;
using System.Text;
using Peter.Repositories.Interfaces;

namespace Peter.Repositories.Implementations
{
    internal class FileSystemFacade : IFileSystemFacade
    {
        public void Backup(string fullPath, string backupFullPath)
        {
            var backupDir = Path.GetDirectoryName(backupFullPath);

            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            File.Move(fullPath, backupFullPath);
        }

        public string Load(string fullPath) => File.ReadAllText(fullPath, Encoding.UTF8);

        public void Save(string fullPath, string content)
        {
            var dir = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(fullPath, content, Encoding.UTF8);
        }
    }
}
