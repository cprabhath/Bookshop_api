using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Data;
using Bookshop_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_api.BusinessLayer.Services
{
    public class BookServices : IBook
    {
        private readonly ApplicationDBContext _context;
        public BookServices(ApplicationDBContext context)
        {
            _context = context;
        }
        public String AddBook(Book book)
        {
            try
            {
                _context.Books.Add(book);
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

        public async Task<string> DeleteBook(int id)
        {
            try
            {
                var result = await _context.Books.FindAsync(id);
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

        public IEnumerable<Book> GetAllBooks()
        {
            try
            {
                var result = _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Where(b => b.DeletedAt == null)
                    .AsEnumerable();
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

        public async Task<Book> GetBookById(int id)
        {
            try
            {
                var result = await _context.Books.
                    Include(b => b.Author).
                    Include(b => b.Category).
                    FirstOrDefaultAsync(b => b.Id == id);
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

        public async Task<string> UpdateBook(int id, Book book)
        {
            try
            {
                var result = await _context.Books.FindAsync(id);
                if (result != null)
                {
                    result.Image = book.Image;
                    result.ISBN = book.ISBN;
                    result.Title = book.Title;
                    result.Author = book.Author;
                    result.Description = book.Description;
                    result.Category = book.Category;
                    result.Language = book.Language;
                    result.Price = book.Price;
                    result.UpdateAt = DateTime.Now;
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
    }
}
