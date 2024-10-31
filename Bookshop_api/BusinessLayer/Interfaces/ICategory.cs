using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface ICategory
    {
        IEnumerable<Category> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        String AddCategory(Category category);
        Task<string> UpdateCategory(int id, Category category);
        Task<string> DeleteCategory(int id);
    }
}
