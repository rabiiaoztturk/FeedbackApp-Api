using FeedbackApp.Data;
using FeedbackApp.Dto;
using FeedbackApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UpvoteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UpvoteController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public ActionResult<List<DtoUpvoteResponse>> GetUpvote()
        {
            var upvote = _context.Upvotes
                .Select(logins => new DtoUpvoteResponse
                {
                    Id = logins.Id,
                    FeedbackId = logins.FeedbackId,
                    UserId = logins.UserId
                })
                .ToList();

            return Ok(upvote);
        }

        [HttpPost]
        public ActionResult<Upvote> AddUpvote([FromBody] DtoUpvoteRequest upvoteRequest)
        {
            var loginUser = _context.Users.FirstOrDefault(u => u.Id == upvoteRequest.UserId);

            if (loginUser == null)
            {
                return Unauthorized(new { message = "Giriş yapmanız gerekiyor." });
            }

            var upvote = new Upvote
            {
                FeedbackId = upvoteRequest.FeedbackId,
                UserId = upvoteRequest.UserId
            };

            _context.Upvotes.Add(upvote);
            _context.SaveChanges();

            var response = new DtoUpvoteResponse
            {
                Id = upvote.Id,
                FeedbackId = upvote.FeedbackId,
                UserId = upvote.UserId
            };

            return Ok(response);
        }
        // Kullanicinin attigi upvote i geri cekmesi icin yapildi FE den gelen istek
        [HttpDelete("{id}")]
        public bool DeleteUpvote(int id)
        {
            var upvote = _context.Upvotes.Find(id);
            if (upvote is null)
                return false;
            _context.Upvotes.Remove(upvote);
            _context.SaveChanges();
            return true;
        }
    }
}
