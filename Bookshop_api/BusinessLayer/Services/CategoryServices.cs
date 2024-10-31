using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Data;
using Bookshop_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_api.BusinessLayer.Services
{
    public class CategoryServices : ICategory
    {
        private readonly ApplicationDBContext _context;

        public CategoryServices(ApplicationDBContext context)
        {
            _context = context;
        }

        public string AddCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return "OK";
            }
            catch (DbUpdateException ex)
            {
                return ex.InnerException!.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeleteCategory(int id)
        {
            try
            {
                var result = await _context.Categories.FindAsync(id);
                if (result != null)
                {
                    result.DeletedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return "OK";
                }
                else
                {
                    return "Book not found";
                }
            }
            catch (DbUpdateException ex)
            {
                return ex.InnerException!.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            try
            {
                var result = _context.Categories
                    .Where(b => b.DeletedAt == null)
                    .ToList();
                return result;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Category> GetCategoryById(int id)
        {
            try
            {
                var result = await _context.Categories.FindAsync(id);
                return result!;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdateCategory(int id, Category category)
        {
            try
            {
                var result = await _context.Categories.FindAsync(id);
                if (result != null)
                {
                    result.Name = category.Name;
                    result.Description = category.Description;
                    await _context.SaveChangesAsync();
                    return "OK";
                }
                else
                {
                    return "Category not found";
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
