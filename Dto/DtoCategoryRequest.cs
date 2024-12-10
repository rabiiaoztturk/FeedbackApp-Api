using FeedbackApp.Models;

namespace FeedbackApp.Dto
{
    public class DtoCategoryRequest
    {
        public string Name { get; set; }
    }

    public class DtoCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
