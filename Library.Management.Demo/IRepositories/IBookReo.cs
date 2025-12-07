using Library.Management.Demo.Dtos;
using Library.Management.Demo.Models;

namespace Library.Management.Demo.IRepositories
{
    public interface IBookReo
    {
        IQueryable<Book> GetList();
        Task<Book> GetById(int id);
        Task<bool> Create(Book book);
        Task<bool> Update(Book book);
        Task<bool> Remove(Book book);
        Task<bool> CheckExistense(string title);

        Task<List<BookRatingDto>> GetBooksRating();
    }
}
