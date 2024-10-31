using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Services
{
    public class FeedbackServices : IFeedback
    {
        public Task<Feedback> AddFeedback(Feedback feedback)
        {
            throw new NotImplementedException();
        }

        public Task<Feedback> DeleteFeedback(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Feedback> GetFeedback(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Feedback>> GetFeedbacks()
        {
            throw new NotImplementedException();
        }

        public Task<Feedback> UpdateFeedback(int id, Feedback feedback)
        {
            throw new NotImplementedException();
        }
    }
}
