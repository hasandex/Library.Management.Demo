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
        [HttpGet]
        public async Task<IActionResult> GetBooks(string? searchKey)
        {
            var books = await _bookeService.GetBooks(searchKey);
            return Ok(books);
        }

        [HttpPost]
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
    }
}
