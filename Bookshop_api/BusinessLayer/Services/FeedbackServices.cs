using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Data;
using Bookshop_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_api.BusinessLayer.Services
{
    public class FeedbackServices : IFeedback
    {
        private readonly ApplicationDBContext _context;
        public FeedbackServices(ApplicationDBContext context) 
        {
            _context = context;
        }
        public async Task<string> AddFeedback(Feedback feedback)
        {
            try
            {
                await _context.Feedbacks.AddAsync(feedback);
                await _context.SaveChangesAsync();
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

        public async Task<string> DeleteFeedback(int id)
        {
            try
            {
                var result = await _context.Feedbacks.FindAsync(id);
                if (result != null)
                {
                    result.DeletedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return "OK";
                }
                else
                {
                    return "Feedback not found";
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

        public async Task<Feedback> GetFeedback(string email)
        {
            try
            {
                var result = await _context.Feedbacks
                    .Where(o => o.DeletedAt == null)
                    .FirstOrDefaultAsync(o => o.email == email);

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

        public IEnumerable<Feedback> GetFeedbacks()
        {
            try
            {
                var results = _context.Feedbacks.AsEnumerable();
                return results;
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

        public async Task<string> UpdateFeedback(int id, Feedback feedback)
        {
            try
            {
                var result = _context.Feedbacks.FindAsync(id);
                if (result != null)
                {
                    result.Result!.message = feedback.message;
                    result.Result.name = feedback.name;
                    result.Result.UpdateAt = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return "OK";
                }
                else
                {
                    return "Feedback not found";
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
