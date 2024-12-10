using System.ComponentModel.DataAnnotations.Schema;

namespace FeedbackApp.Models
{
    public enum Status
    {
        Planned = 1,
        InProgress = 2,
        Live = 3,
    }
    public class Feedback
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int CommentCount { get; set; }
        public List<Upvote> Upvotes { get; set; }
        public int UpvoteCount { get; set; }
        public Status? Status { get; set; }
    }
}
