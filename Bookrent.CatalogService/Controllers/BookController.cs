using Bookrent.CatalogService.Services;
using Microsoft.AspNetCore.Mvc;
using BookRent.OutputModels;

namespace Bookrent.CatalogService.Controllers
{
    [ApiController]
    [Route("api/book")]
    public class BookController : Controller
    {
        private readonly BooksService _service;

        public BookController(BooksService service)
        {
            this._service = service;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var booksRes = await _service.GetAll();
            if (booksRes.BadRequest && booksRes.Errors is not null)
                    return BadRequest();
           if (booksRes.NotFound && booksRes.Errors is not null)
                    return NotFound();   
           
            var books = booksRes.Result;
            return Ok(new OutputBookList(books));
        }

        [HttpGet]
        [Route("getbyid")]
        public async Task<IActionResult> GetById(string id)
        {
            var booksRes = await _service.Get(id);
            if (booksRes.BadRequest && booksRes.Errors is not null)
                return BadRequest();
            if (booksRes.NotFound && booksRes.Errors is not null)
                return NotFound();

            var books = booksRes.Result;
            return Ok(new OutputBookItem(books));
        }

        /*[HttpPost]
        [Route("rentbook")]
        public async Task<IActionResult> RentBookById([FromBody] string bookId,  [FromBody] string userLogin)
        {
            
        }*/
    }
}
