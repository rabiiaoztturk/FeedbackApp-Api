using System.ComponentModel.DataAnnotations.Schema;

namespace FeedbackApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int FeedbackId { get; set; }
        public Feedback Feedback { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
