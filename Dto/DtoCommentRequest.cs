using FeedbackApp.Models;

namespace FeedbackApp.Dto
{
    public class DtoCommentRequest
    {
        public int UserId { get; set; }
        public int FeedbackId { get; set; }
        public string Description { get; set; }

    }
    public class DtoCommentResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }

    public class DtoFeedbackIdResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public List<DtoCommentResponse> Comments { get; set; }
    }
}
