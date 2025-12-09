using Library.Management.Demo.Dtos;
using Library.Management.Demo.Extension;
using Library.Management.Demo.IRepositories;
using Library.Management.Demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Library.Management.Demo.Repositories
{
    public class BookRepo : IBookReo
    {
        private readonly AppDbContext _context;

        public BookRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExistense(string title)
        {
            var book = await _context.Books.FirstOrDefaultAsync(a => a.Title == title);
            return book != null ? false : true;
        }

        public async Task<bool> Create(Book book)
        {
            await _context.Books.AddAsync(book);
            return await _context.SaveChangesAsync() > 0 ? true : false ;
        }

        public async Task<List<BookRatingDto>> GetBooksRating()
        {
            var result = new List<BookRatingDto>();
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "GetBookAverageRatings";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var bookRating = new BookRatingDto()
                            {
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                PublisherName = reader.GetString(reader.GetOrdinal("publisherName")),
                                AverageRating = reader.GetInt32(reader.GetOrdinal("AverageRating"))
                            };
                            result.Add(bookRating);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Bookdto>> GetBooksSql(string? searchKey)
        {
            var result = new List<Bookdto>();
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand() )
                {
                    command.CommandText = "GetBooksList"; // Name of the stored procedure
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // Add the search key parameter
                    var param = command.CreateParameter();
                    param.ParameterName = "@searchKey"; // Must match the parameter name in the stored procedure
                    param.Value = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
                    command.Parameters.Add(param);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var book = new Bookdto
                            {
                                Title = reader.GetString(reader.GetOrdinal("book_title")),
                                Author = reader.GetString(reader.GetOrdinal("author_name")),
                                Category = reader.GetString(reader.GetOrdinal("category_name")),
                                Publisher = reader.GetString(reader.GetOrdinal("publisher_name")),
                                PublishedYear = reader.GetDateTime(reader.GetOrdinal("PublishedYear")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            };
                            var bookEdition = reader.GetInt32(reader.GetOrdinal("book_copy"));
                            if (!book.BookEditions.Contains(bookEdition))
                            {
                                book.BookEditions.Add(bookEdition); // Add if not already present
                            }

                            var libraryName = reader.GetString(reader.GetOrdinal("library_name"));
                            if (!book.Libraries.Contains(libraryName))
                            {
                                book.Libraries.Add(libraryName);
                            }
                            var review = reader.GetString(reader.GetOrdinal("Comment"));
                            if (!book.Reviews.Contains(review))
                            {
                                book.Reviews.Add(review);
                            }
                            result.Add(book);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<Book> GetById(int id)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Reviews)
                .Include(b=> b.BookEditions)
                .Include(b => b.BookLibraries)
                .ThenInclude(bl => bl.Library)
                .FirstOrDefaultAsync(b => b.Id == id);   
        }

        public IQueryable<Book> GetList()
        {
            var query = _context.Books
                .Include(b=>b.Category)
                .Include(b=>b.Author)
                .Include(b=>b.Publisher)
                .Include(b=>b.Reviews)
                .Include(b => b.BookEditions)
                .Include(b=>b.BookLibraries)
                .ThenInclude(bl=>bl.Library)
                .AsNoTracking();
            return query;
        }

        public async Task<bool> Remove(Book book)
        {
            _context.Books.Remove(book);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> Update(Book book)
        {
            _context.Books.Update(book);
            return await _context.SaveChangesAsync() > 0 ? true :false ;
        }
    }
}
