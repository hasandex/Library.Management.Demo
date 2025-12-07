using Library.Management.Demo.Dtos;

namespace Library.Management.Demo.Services
{
    public interface IBookeService
    {
        Task<List<Bookdto>> GetBooks(string? searchKey);
        Task<Bookdto> GetBookById(int id);
        Task<bool> CreateBook(CreateUpdateBookDto dto);
        Task<bool> UpdateBook(CreateUpdateBookDto dto);
        Task<bool> DeleteBook(int id);

        Task<List<BookRatingDto>> GetBooksRate();
        
    }
}
