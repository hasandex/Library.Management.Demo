using Library.Management.Demo.Models;
using Microsoft.IdentityModel.Tokens;

namespace Library.Management.Demo.Extension
{
    public static class BookExtension
    {
        public static IQueryable<Book> BookFilter(this IQueryable<Book> query, string? searchKey)
        {
            if (!searchKey.IsNullOrEmpty())
            {
                int yearKey;
                int.TryParse(searchKey, out yearKey);
                var key = searchKey?.ToLower();
                query = query.Where(b =>
                b.Title.ToLower().Contains(key) ||
                b.Category.Name.ToLower().Contains(key) ||
                b.Author.Name.ToLower().Contains(key) ||
                b.Publisher.Name.ToLower().Contains(key) ||
                 b.BookLibraries.Any(l => l.Library.Name.ToLower().Contains(key)) ||
                b.PublishedYear.Value.Year == yearKey);
                return query;
            }
            return query;
        }
    }
}
