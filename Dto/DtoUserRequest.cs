using System.ComponentModel.DataAnnotations.Schema;

namespace FeedbackApp.Dto
{
    public class DtoUserRequest
    {
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
    public class DtoUserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImageUrl { get; set; }

    }
}
