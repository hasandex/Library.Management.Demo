using Library.Management.Demo.Dtos;
using Library.Management.Demo.Extension;
using Library.Management.Demo.IRepositories;
using Library.Management.Demo.Models;
using Library.Management.Demo.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace Library.Management.Demo.Services
{
    public class BookService : IBookeService
    {
        private readonly IBookReo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IPublisherRepo _publisherRepo;
        private readonly ILIbraryRepo _IbraryRepo;
        private readonly IBookLibraryRepo _bookLibraryRepo;
        private readonly IMemoryCache _memoryCache;

        public BookService(IBookReo bookRepo, IAuthorRepo authorRepo,
            ICategoryRepo categoryRepo, IPublisherRepo publisherRepo,
            ILIbraryRepo ibraryRepo, IBookLibraryRepo bookLibraryRepo,
            IMemoryCache memoryCache)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _categoryRepo = categoryRepo;
            _publisherRepo = publisherRepo;
            _IbraryRepo = ibraryRepo;
            _bookLibraryRepo = bookLibraryRepo;
            _memoryCache = memoryCache;
        }

        public async Task<bool> CreateBook(CreateUpdateBookDto dto)
        {
            //Validation
            var isExisted = await _bookRepo.CheckExistense(dto.Title);
            if (!isExisted)
                throw new ArgumentException("Book is already existed");

            var autor = await _authorRepo.CheckExistence(dto.AuthorId);
            if(!autor)
                throw new ArgumentException("Author is not existed",nameof(autor));

            var category = await _categoryRepo.CheckExistence(dto.CategoryId);
            if (!category)
                throw new ArgumentException("Category is not existed", nameof(category));

            var publisher = await _publisherRepo.CheckExistence(dto.PublisherId);
            if (!publisher)
                throw new ArgumentException("Publisher is not existed", nameof(publisher));

            foreach (var libId in dto.LibrariesId)
            {
                var library = await _IbraryRepo.CheckExistence(libId);
                if (!library)
                    throw new ArgumentException($"Library with the id {libId} is not existed", nameof(library));
            }

            var book = new Book()
            {
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                AuthorId = dto.AuthorId,
                PublishedYear = dto.PublishedYear,
                PublisherId = dto.PublisherId,
                Quantity = dto.Quantity,
            };
            foreach (var libId in dto.LibrariesId)
            {
                book.BookLibraries.Add(new BookLibrary() { LibraryId = libId });
            }
            var isCreated = await _bookRepo.Create(book);
            _memoryCache.Remove(Helper.BookCacheKey);
            await _memoryCache.Set(Helper.BookCacheKey,GetBooks(""),TimeSpan.FromMinutes(5));
            return isCreated;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _bookRepo.GetById(id);
            if (book == null)
                throw new KeyNotFoundException($"book with id {id} not found");
            var removed = await _bookRepo.Remove(book);
            _memoryCache.Remove(Helper.BookCacheKey);
            await _memoryCache.Set(Helper.BookCacheKey, GetBooks(""), TimeSpan.FromMinutes(5));
            return removed;
        }

        public async Task<Bookdto> GetBookById(int id)
        {
            var book = await _bookRepo.GetById(id);
            if (book == null)
                throw new KeyNotFoundException($"Book with id {id} not found");
            var bookdto = new Bookdto()
            {
                Title = book.Title,
                Author = book.Author.Name,
                Category = book.Category.Name,
                PublishedYear = book.PublishedYear,
                Publisher = book.Publisher.Name,
                Quantity = book.Quantity,
                BookEditions = book.BookEditions.Select(be => be.CopyNumber).ToList(),
                Libraries = book.BookLibraries.Select(bl=>bl.Library.Name).ToList(),
                Reviews = book.Reviews.Select(r=>r.Comment).ToList()
            };
            return bookdto;
        }

        public async Task<List<Bookdto>> GetBooks(string? searchKey)
        {
            var books = new List<Bookdto>();
            string cacheKey = Helper.BookCacheKey + (searchKey ?? "");
            if (!_memoryCache.TryGetValue(cacheKey, out books))
            {
                var query = await _bookRepo.GetList().BookFilter(searchKey).ToListAsync();
                books = query.Select(b => new Bookdto()
                {
                    Title = b.Title,
                    Author = b.Author.Name,
                    Quantity = b.Quantity,
                    PublishedYear = b.PublishedYear,
                    Publisher = b.Publisher.Name,
                    Category = b.Category.Name,
                    BookEditions = b.BookEditions.Select(be => be.CopyNumber).ToList(),
                    Libraries = b.BookLibraries.Select(bl => bl.Library.Name).ToList(),
                    Reviews = b.Reviews.Select(r => r.Comment).ToList(),
                }).ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _memoryCache.Set(cacheKey, books, cacheEntryOptions);
            }        
            return books;
        }

        public async Task<List<BookRatingDto>> GetBooksRate()
        {
            return await _bookRepo.GetBooksRating();
        }

        public async Task<List<Bookdto>> GetBooksSql(string searchKey)
        {
            return await _bookRepo.GetBooksSql(searchKey);
        }

        public async Task<bool> UpdateBook(CreateUpdateBookDto dto)
        {
            var book = await _bookRepo.GetById(dto.BookId);
            if (book == null)
                throw new KeyNotFoundException($"Book with the id {dto.BookId} not found");

            var autor = await _authorRepo.CheckExistence(dto.AuthorId);
            if (!autor)
                throw new ArgumentException("Author is not existed", nameof(autor));

            var category = await _categoryRepo.CheckExistence(dto.CategoryId);
            if (!category)
                throw new ArgumentException("Category is not existed", nameof(category));

            var publisher = await _publisherRepo.CheckExistence(dto.PublisherId);
            if (!publisher)
                throw new ArgumentException("Publisher is not existed", nameof(publisher));

            foreach (var libId in dto.LibrariesId)
            {
                var library = await _IbraryRepo.CheckExistence(libId);
                if (!library)
                    throw new ArgumentException($"Library with the id {libId} is not existed", nameof(library));
            }

            book.Title = dto.Title;
            book.AuthorId = dto.AuthorId;
            book.PublisherId = dto.PublisherId;
            book.CategoryId = dto.CategoryId;
            book.Quantity = dto.Quantity;
            book.PublishedYear = dto.PublishedYear;

            //update the related libraries

            //fetch the related libraries

            var existeingLibraries = book.BookLibraries
                .Select(bl => bl.LibraryId).ToList();

            //remove what is no longer associated

            var updatedLibraries = new HashSet<int>(dto.LibrariesId);

            foreach (var libraryId in existeingLibraries)
            {
                if (!updatedLibraries.Contains(libraryId))
                    await _bookLibraryRepo.RemoveByLibraryId(libraryId);
            }

            //Add new BookLibraries that don't already exist

            foreach (var libId in dto.LibrariesId)
            {
                if (!existeingLibraries.Contains(libId))
                {
                    await _bookLibraryRepo.Insert(libId, dto.BookId);
                }
            }
            var updated = await _bookRepo.Update(book);
            _memoryCache.Remove(Helper.BookCacheKey);
            await _memoryCache.Set(Helper.BookCacheKey, GetBooks(""), TimeSpan.FromMinutes(5));
            return updated;

        }
    }
}
