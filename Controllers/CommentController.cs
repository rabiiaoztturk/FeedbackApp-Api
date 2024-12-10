using FeedbackApp.Data;
using FeedbackApp.Dto;
using FeedbackApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<DtoCommentResponse>> GetComments()  
        {
            var comments = _context.Comments
                .Select(comments => new DtoCommentResponse
                {
                    Id = comments.Id,
                    Description = comments.Description
                })
                .ToList();

            return Ok(comments);
        }

        [HttpGet("{id}")]
        public ActionResult<DtoCommentResponse> GetComment(int id)
        {
            var comments = _context.Comments.FirstOrDefault(x => x.Id == id);

            if (comments is null)
                return NotFound();

            var response = new DtoCommentResponse
            {
                Id = comments.Id,
                Description = comments.Description
            };

            return Ok(response);
        }

        [HttpGet("{feedbackId}")]
        public ActionResult<DtoFeedbackResponse> GetFeedbackComments(int feedbackId)
        {
            var feedback = _context.Feedbacks
                .Include(f => f.Comments)
                .FirstOrDefault(f => f.Id == feedbackId);

            if (feedback is null)
                return NotFound();

            var response = new DtoFeedbackIdResponse
            {
                Id = feedback.Id,
                Title = feedback.Title,
                Description = feedback.Description,
                CategoryId = feedback.CategoryId,
                UserId = feedback.UserId,
                Status = (int)feedback.Status,
                Comments = feedback.Comments.Select(c => new DtoCommentResponse
                {
                    Id = c.Id,
                    Description = c.Description,
                    UserId = c.UserId
                }).ToList()
            };

            return Ok(response);
        }


        [HttpPost]
        public ActionResult<Comment> AddComment([FromBody] DtoCommentRequest commentRequest)
        {
            var loginUser = _context.Users.FirstOrDefault(u => u.Id == commentRequest.UserId);

            if (loginUser == null)
            {
                return Unauthorized(new { message = "Giriş yapmanız gerekiyor." });
            }

            var comments = new Comment
            {
                Description = commentRequest.Description,
                FeedbackId = commentRequest.FeedbackId,
                UserId = commentRequest.UserId

            };

            _context.Comments.Add(comments);
            _context.SaveChanges();

            var response = new DtoCommentResponse
            {
                Id = comments.Id,
                Description = comments.Description
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<DtoCommentResponse> EditComment(int id, [FromBody] DtoCommentRequest commentRequest)
        {
            // Yorumun mevcut olup olmadığını kontrol et
            var comment = _context.Comments
                .Include(c => c.User)
                .FirstOrDefault(c => c.Id == id);

            if (comment == null)
                return NotFound(new { message = "Yorum bulunamadı." });

            comment.Description = commentRequest.Description;
            comment.UserId = commentRequest.UserId;

            _context.SaveChanges();

            var response = new DtoCommentResponse
            {
                Id = comment.Id,
                Description = comment.Description,
                UserId = comment.UserId,
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public bool DeleteComment(int id)
        {
            var comments = _context.Comments.Find(id);
            if (comments is null)
                return false;
            _context.Comments.Remove(comments);
            _context.SaveChanges();
            return true;
        }
    }
}
