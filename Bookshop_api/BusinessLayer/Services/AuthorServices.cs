using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Data;
using Bookshop_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_api.BusinessLayer.Services
{
    public class AuthorServices : IAuthor
    {
        private readonly ApplicationDBContext _context;
        public AuthorServices(ApplicationDBContext context)
        {
            _context = context;
        }

        public string AddAuthor(Author author)
        {
            try
            {
                _context.Authors.Add(author);
                _context.SaveChanges();
                return "OK";
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

        public async Task<string> DeleteAuthor(int id)
        {
            try
            {
                var result = await _context.Authors.FindAsync(id);
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

        public async Task<Author> GetAuthorById(int id)
        {
            try
            {
                var result = await _context.Authors.FindAsync(id);
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

        public IEnumerable<Author> GetAllAuthors()
        {
            try
            {
                var result = _context.Authors
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

        public async Task<string> UpdateAuthor(int id, Author author)
        {
            try
            {
                var result = await _context.Authors.FindAsync(id);
                if (result != null)
                {
                    result.Name = author.Name;
                    result.MobileNumber = author.MobileNumber;
                    result.Email = author.Email;
                    result.Address = author.Address;
                    await _context.SaveChangesAsync();
                    return "OK";
                }
                else
                {
                    return "Author not found";
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
