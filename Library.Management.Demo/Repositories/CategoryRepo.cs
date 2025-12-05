using Library.Management.Demo.IRepositories;
using Library.Management.Demo.Models;

namespace Library.Management.Demo.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;

        public CategoryRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExistence(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category != null ? true : false;
        }
    }
}
