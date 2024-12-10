using FeedbackApp.Data;
using FeedbackApp.Dto;
using FeedbackApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<DtoLoginResponse>> GetLogin()
        {
            var login = _context.Logins
                .Select(logins => new DtoLoginResponse
                {
                    Id = logins.Id,
                    Nickname = logins.Nickname,
                    Password = logins.Password
                })
                .ToList();

            return Ok(login);
        }

        [HttpGet("{id}")]
        public ActionResult<DtoLoginResponse> GetLogin(int id)
        {
            var logins = _context.Logins.FirstOrDefault(x => x.Id == id);

            if (logins is null)
                return NotFound();

            var response = new DtoLoginResponse
            {
                Id = logins.Id,
                Nickname = logins.Nickname,
                Password = logins.Password
            };

            return Ok(response);
        }

        [HttpPost]
        public ActionResult<Login> AddLogin([FromBody] DtoLoginRequest loginRequest)
        {
            var user = _context.Users.FirstOrDefault(u =>
                    u.Nickname == loginRequest.Nickname &&
                    u.Password == loginRequest.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Geçersiz Kullanıcı. Böyle bir kullanıcı bulunamadı." });
            }

            var logins = new Login
            {
                Nickname = loginRequest.Nickname,
                Password = loginRequest.Password
            };

            _context.Logins.Add(logins);
            _context.SaveChanges();

            var response = new DtoLoginResponse
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Password = user.Password
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Logout([FromBody] DtoLoginLogout logoutRequest)
        {
            var user = _context.Logins.FirstOrDefault(l =>
                l.Id == logoutRequest.Id);

            if (user == null)
            {
                return BadRequest(new { message = "Geçersiz kullanıcı. Böyle bir kullanıcı bulunamadı." });
            }

            var login = _context.Logins.FirstOrDefault(l => l.Id == logoutRequest.Id);

            if (login != null)
            {
                _context.Logins.Remove(login);
                _context.SaveChanges();
            }

            return Ok(new { message = "Oturum başarıyla kapatıldı." });
        }
    }
} 
