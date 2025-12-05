using Library.Management.Demo.IRepositories;
using Library.Management.Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Management.Demo.Repositories
{
    public class BookLibraryRepo : IBookLibraryRepo
    {
        private readonly AppDbContext _context;

        public BookLibraryRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Insert(int libraryId, int bookId)
        {
            _context.BookLibraries.Add(new BookLibrary() { BookId = bookId, LibraryId = libraryId });
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> RemoveByLibraryId(int libraryId)
        {
            var bookLibrary = await _context.BookLibraries.FirstOrDefaultAsync(bl=>bl.LibraryId == libraryId);
            _context.BookLibraries.Remove(bookLibrary);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
