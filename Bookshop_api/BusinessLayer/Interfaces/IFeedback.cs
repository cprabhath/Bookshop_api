using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface IFeedback
    {
        Task<IEnumerable<Feedback>> GetFeedbacks();
        Task<Feedback> GetFeedback(int id);
        Task<Feedback> AddFeedback(Feedback feedback);
        Task<Feedback> UpdateFeedback(int id, Feedback feedback);
        Task<Feedback> DeleteFeedback(int id);
    }
}
