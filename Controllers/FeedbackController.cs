using FeedbackApp.Data;
using FeedbackApp.Dto;
using FeedbackApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<DtoFeedbackResponse>> GetFeedbacks()
        {
            var feedbacks = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Category)
                .Include(f => f.Comments)
                .Select(feedback => new DtoFeedbackResponse
                {
                    Id = feedback.Id,
                    Title = feedback.Title,
                    Description = feedback.Description,
                    Status = feedback.Status.HasValue ? (int)feedback.Status : 0, 
                    CategoryId = feedback.CategoryId,
                    UserId = feedback.UserId,
                    CommentCount = feedback.Comments.Count,
                    UpvoteCount = feedback.Upvotes.Count
                })
                .ToList();

            return Ok(feedbacks);
        }


        [HttpGet("{id}")]
        public ActionResult<DtoFeedbackResponse> GetFeedback(int id)
        {
            var feedback = _context.Feedbacks
                .Include(f => f.Category)
                .Include(f => f.User)
                .FirstOrDefault(x => x.Id == id);

            if (feedback is null)
                return NotFound();

            var response = new DtoFeedbackResponse
            {
                Id = feedback.Id,
                Title = feedback.Title,
                Description = feedback.Description,
                Status = (int)feedback.Status,
                CategoryId = feedback.Category.Id,
                UserId = feedback.User.Id
            };

            return Ok(response);
        }

        [HttpPost]
        public ActionResult<DtoFeedbackResponse> AddFeedback([FromBody] DtoFeedbackRequest feedbackRequest)
        {
            var loginUser = _context.Users.FirstOrDefault(u => u.Id == feedbackRequest.UserId);

            if (loginUser == null)
            {
                return Unauthorized(new { message = "Giriş yapmanız gerekiyor." });
            }


            var category = _context.Categories.FirstOrDefault(c => c.Id == feedbackRequest.CategoryId);
            if (category == null)
            {
                return BadRequest(new { message = "Geçersiz CategoryId. Böyle bir kategori bulunamadı." });
            }

            var feedback = new Feedback
            {
                Title = feedbackRequest.Title,
                Description = feedbackRequest.Description,
                CategoryId = feedbackRequest.CategoryId,
                UserId = feedbackRequest.UserId,
                Status = null
            };

            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            var savedFeedback = _context.Feedbacks
                .Include(f => f.Category)
                .Include(f => f.User)
                .FirstOrDefault(f => f.Id == feedback.Id);

            var response = new DtoFeedbackResponse
            {
                Id = savedFeedback.Id,
                Title = savedFeedback.Title,
                Description = savedFeedback.Description,
                Status = (int)feedback.Status,
                CategoryId = feedback.CategoryId,
                UserId = feedback.UserId,
            };

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public bool DeleteFeedback(int id)
        {
            var feedback = _context.Feedbacks.Find(id);
            if (feedback is null)
                return false;
            _context.Feedbacks.Remove(feedback);
            _context.SaveChanges();
            return true;
        }
    }
}
