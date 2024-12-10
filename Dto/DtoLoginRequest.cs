namespace FeedbackApp.Dto
{
    public class DtoLoginRequest
    {
        public string Nickname { get; set; }
        public string Password { get; set; }
    }

    public class DtoLoginResponse
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
    }

    public class DtoLoginLogout
    {
        public int Id { get; set; }
    }
}
