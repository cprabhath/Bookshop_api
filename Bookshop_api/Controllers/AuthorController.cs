using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthor _authorService;

        public AuthorController(IAuthor authorService)
        {
            _authorService = authorService;
        }

        // ====================== Get All Authors ======================
        [HttpGet]
        public IActionResult Get()
        {
            var result = _authorService.GetAllAuthors();
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "No Authors Found");
            }
        }
        // ===========================================================

        // =========================== Add Author ======================
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public IActionResult Post([FromBody] Author author)
        {
            var result = _authorService.AddAuthor(author);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Author Added Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        // ============================================================

        // ====================== Get Author By Id ======================
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _authorService.GetAuthorById(id);

            if (result.Result != null)
            {
                return Ok(result.Result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Author Not Found");
            }
        }
        // ============================================================

        // ======================== Update Author =======================
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Author author)
        {
            var result = _authorService.UpdateAuthor(id, author);

            if (result.Result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Author Updated Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Result);
            }
        }
        // ============================================================

        // ======================== Delete Author =======================
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _authorService.DeleteAuthor(id);

            if (result.Result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Author Deleted Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Result);
            }
        }
    }
}
