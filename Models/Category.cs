namespace FeedbackApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Feedback> Feedbacks { get; set; }
    }
}
