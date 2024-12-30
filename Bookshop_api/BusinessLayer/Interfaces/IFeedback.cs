using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface IFeedback
    {
        IEnumerable<Feedback> GetFeedbacks();
        Task<Feedback> GetFeedback(string email);
        Task<string> AddFeedback(Feedback feedback);
        Task<string> UpdateFeedback(int id, Feedback feedback);
        Task<string> DeleteFeedback(int id);
        Task<string> SendMessage(SendMessage sendMessage);
    }
}
