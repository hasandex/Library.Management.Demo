using Library.Management.Demo.IRepositories;
using Library.Management.Demo.Models;

namespace Library.Management.Demo.Repositories
{
    public class LibraryRepo : ILIbraryRepo
    {
        private readonly AppDbContext _context;

        public LibraryRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExistence(int id)
        {
            var library = await _context.Libraries.FindAsync(id);
            return library != null ? true : false;
        }
    }
}
