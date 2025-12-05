using Library.Management.Demo.Models;

namespace Library.Management.Demo.IRepositories
{
    public interface IBookReo
    {
        IQueryable<Book> GetList();
        Task<bool> Create(Book book);
    }
}
