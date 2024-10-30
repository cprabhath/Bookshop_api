using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface IBook
    {
        IEnumerable<Book> GetAllBooks();
        String AddBook(Book book);
        Task<string> UpdateBook(int id, Book book);
        Task<string> DeleteBook(int id);
        Task<Book> GetBookById(int id);
    }
}
