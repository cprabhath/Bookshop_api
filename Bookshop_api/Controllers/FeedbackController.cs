using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedback _feedback;

        public FeedbackController(IFeedback feedback)
        {
            _feedback = feedback;
        }

        // Add feedback
        [HttpPost("AddFeedback")]
        public async Task<IActionResult> AddFeedback([FromBody] Feedback feedback)
        {
            if (feedback == null)
            {
                return BadRequest("Feedback cannot be null.");
            }

            var result = await _feedback.AddFeedback(feedback);

            if (result == "OK")
            {
                return Ok("Feedback added successfully.");
            }

            return BadRequest(result);
        }

        // Update feedback
        [HttpPut("UpdateFeedback/{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] Feedback feedback)
        {
            if (feedback == null)
            {
                return BadRequest("Feedback cannot be null.");
            }

            var result = await _feedback.UpdateFeedback(id, feedback);

            if (result == "OK")
            {
                return Ok("Feedback updated successfully.");
            }

            return BadRequest(result);
        }

        // Delete feedback
        [HttpDelete("DeleteFeedback/{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var result = await _feedback.DeleteFeedback(id);

            if (result == "OK")
            {
                return Ok("Feedback deleted successfully.");
            }

            return BadRequest(result);
        }

        // Get feedback by email
        [HttpGet("GetFeedback/{email}")]
        public async Task<IActionResult> GetFeedback(string email)
        {
            var feedback = await _feedback.GetFeedback(email);

            if (feedback != null)
            {
                return Ok(feedback);
            }

            return NotFound("Feedback not found.");
        }

        // Get all feedbacks
        [HttpGet("GetFeedbacks")]
        public IActionResult GetFeedbacks()
        {
            var feedbacks = _feedback.GetFeedbacks();

            if (feedbacks != null && feedbacks.Any())
            {
                return Ok(feedbacks);
            }

            return NotFound("No feedbacks found.");
        }

        // Send message
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessage sendMessage)
        {
            if (sendMessage == null)
            {
                return BadRequest("Message cannot be null.");
            }

            var result = await _feedback.SendMessage(sendMessage);

            if (result == "OK")
            {
                return Ok("Message sent successfully.");
            }

            return BadRequest(result);
        }
    }
}
