using System.ComponentModel.DataAnnotations.Schema;

namespace FeedbackApp.Models
{
    public class Upvote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int FeedbackId { get; set; }
        [ForeignKey("FeedbackId")]
        public Feedback Feedback { get; set; }
    }
}
