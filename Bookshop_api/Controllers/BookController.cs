using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Models;
using Bookshop_api.Validations;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBook _bookServices;
        public BookController(IBook bookServices)
        {
            _bookServices = bookServices;
        }

        // ====================== Get All Books ======================

        [HttpGet]
        public IActionResult Get()
        {
            var result = _bookServices.GetAllBooks();
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "No Books Found");
            }
        }
        // ===========================================================

        // =========================== Add Book ======================
        [HttpPost]
        public IActionResult Post([FromBody] Book book)
        { 
            var result = _bookServices.AddBook(book);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Book Added Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        // ============================================================

        // ====================== Get Book By Id ======================
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _bookServices.GetBookById(id);

            if (result.Result != null)
            {
                return Ok(result.Result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Book not found");
            }
        }
        // ===========================================================

        // ====================== Update Book ========================
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Book book)
        {
            

            var result = _bookServices.UpdateBook(id, book);

            if (result.Result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Book Updated Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Result);
            }
        }
        // ===========================================================

        // ====================== Delete Book ========================
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _bookServices.DeleteBook(id);
            if (result.Result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Book Deleted Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Result);
            }
        }
        // ===========================================================
    }
}
