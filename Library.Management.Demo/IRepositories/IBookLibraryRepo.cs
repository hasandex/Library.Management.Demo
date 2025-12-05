using Library.Management.Demo.Models;
using Library.Management.Demo.Repositories;

namespace Library.Management.Demo.IRepositories
{
    public interface IBookLibraryRepo
    {
        Task<bool> RemoveByLibraryId(int libraryId);

        Task<bool> Insert(int libraryId, int bookId);

    }
}
