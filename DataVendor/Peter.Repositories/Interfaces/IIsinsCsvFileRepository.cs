using Peter.Models.Interfaces;

namespace Peter.Repositories.Interfaces
{
    public interface IIsinsCsvFileRepository
    {
        IIsins Load();
        void Save(IIsins isins);
    }
}