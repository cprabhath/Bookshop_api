using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface IAuthor
    {
        IEnumerable<Author> GetAllAuthors();
        Task<Author> GetAuthorById(int id);
        String AddAuthor(Author author);
        Task<string> UpdateAuthor(int id, Author author);
        Task<string> DeleteAuthor(int id);
    }
}
