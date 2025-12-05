using Library.Management.Demo.Dtos;

namespace Library.Management.Demo.Services
{
    public interface IBookeService
    {
        Task<List<Bookdto>> GetBooks(string? searchKey);
        Task<bool> CreateBook(CreateUpdateBookDto dto);
    }
}
