using System.ComponentModel.DataAnnotations.Schema;

namespace FeedbackApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImageUrl { get; set; } 
        [NotMapped] 
        public IFormFile ProfileImage { get; set; } 
        public List<Comment> Comments { get; set; }
        public List<Feedback> Feedbacks { get; set; }
    }

}
