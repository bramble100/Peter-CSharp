using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IIsinsCsvFileRepository
    {
        INameToIsin Load();
        void Save(INameToIsin isins);
    }
}