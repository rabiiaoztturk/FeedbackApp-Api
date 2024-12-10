namespace FeedbackApp.Dto
{
    public class DtoUpvoteRequest
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
    }
    public class DtoUpvoteResponse
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
    }
}
