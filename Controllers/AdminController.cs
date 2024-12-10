using FeedbackApp.Data;
using FeedbackApp.Dto;
using FeedbackApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPut("EditFeedback/{id}")]
        public ActionResult<DtoFeedbackResponse> EditFeedback(int id, [FromBody] DtoFeedbackRequest feedbackRequest)
        {
            var feedback = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Category)
                .FirstOrDefault(f => f.Id == id);

            if (feedback == null)
                return NotFound(new { message = "Geri bildirim bulunamadı." });

            var category = _context.Categories.Find(feedbackRequest.CategoryId);
            if (category == null)
            {
                return BadRequest(new { message = "Geçersiz kategori." });
            }

            feedback.Title = feedbackRequest.Title;
            feedback.Description = feedbackRequest.Description;
            feedback.CategoryId = feedbackRequest.CategoryId;

            if (feedbackRequest.Status != null) // Null kontrolü
            {
                feedback.Status = (Status?)feedbackRequest.Status; // Atama
            }
            else
            {
                feedback.Status = null; // Status null olarak ayarla (isteğe bağlı)
            }
            _context.SaveChanges();

            var response = new DtoFeedbackResponse
            {
                Id = feedback.Id,
                Title = feedback.Title,
                Description = feedback.Description,
                CategoryId = feedback.CategoryId,
                UserId = feedback.UserId,
                Status = (int)feedback.Status
            };

            return Ok(response);
        }
    }
}
