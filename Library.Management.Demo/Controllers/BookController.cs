using Library.Management.Demo.Dtos;
using Library.Management.Demo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Management.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookeService _bookeService;
        public BookController(IBookeService bookeService)
        {
            _bookeService = bookeService;
        }
        [HttpGet("List")]
        public async Task<IActionResult> Get(string? searchKey)
        {
            var books = await _bookeService.GetBooks(searchKey);
            return Ok(new { count = books.Count , list = books} );
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Bookdto>> GetById(int id)
        {
            try
            {
                var book = await _bookeService.GetBookById(id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateUpdateBookDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
               var result = await _bookeService.CreateBook(dto);
                if (!result)
                    return BadRequest("something went wrong");
                return Ok(new {IsCreated = result, Message = "Book has been added successfully"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(CreateUpdateBookDto dto)
        {
            try
            {
                await _bookeService.UpdateBook(dto);
                return Ok("book has been updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _bookeService.DeleteBook(id);
                if (!deleted)
                    return BadRequest("something went wrong");
                return Ok($"book with id {id} has been deleted");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("BooksRating")]
        public async Task<ActionResult<BookRatingDto>> BooksRating()
        {
            var ratings = await _bookeService.GetBooksRate();
            return Ok(ratings);
        }
        [HttpGet("GetBooksSql")]
        public async Task<IActionResult> GetBooksFromSql(string? searchKey)
        {
            var books = await _bookeService.GetBooksSql(searchKey);
            return Ok(new {count = books.Count, list = books});
        }
    }
}
