using Library.Management.Demo.IRepositories;
using Library.Management.Demo.Models;

namespace Library.Management.Demo.Repositories
{
    public class PublisherRepo : IPublisherRepo
    {
        private readonly AppDbContext _context;

        public PublisherRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExistence(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            return publisher != null ? true : false;
        }
    }
}
