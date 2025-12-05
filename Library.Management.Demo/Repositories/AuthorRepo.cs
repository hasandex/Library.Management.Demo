using Library.Management.Demo.IRepositories;
using Library.Management.Demo.Models;

namespace Library.Management.Demo.Repositories
{
    public class AuthorRepo : IAuthorRepo
    {
        private readonly AppDbContext _context;

        public AuthorRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExistence(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return author != null ? true : false;
        }
    }
}
