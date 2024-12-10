using FeedbackApp.Models;

namespace FeedbackApp.Dto
{
    public class DtoFeedbackRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; } = 0;

    }
    public class DtoFeedbackResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int CommentCount { get; set; }
        public int? UpvoteCount { get; set; }
        public int Status { get; set; }
        public List<DtoCommentResponse> Comments { get; set; }
    }
}
