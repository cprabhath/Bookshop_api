using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;

        public CategoryController(ICategory category)
        {
            _category = category;
        }
        // ====================== Get All Categories ======================
        [HttpGet]
        public IActionResult Get()
        {
            var result = _category.GetAllCategories();
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "No Categories Found");
            }
        }
        // ===========================================================

        // =========================== Add Category ======================
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            var result = _category.AddCategory(category);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Category Added Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        // ============================================================

        // =========================== Update Category ======================
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            var result = await _category.UpdateCategory(id, category);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Category Updated Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        // ============================================================

        // =========================== Delete Category ======================
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _category.DeleteCategory(id);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Category Deleted Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        // ============================================================
    }
}
