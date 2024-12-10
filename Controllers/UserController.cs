using FeedbackApp.Data;
using FeedbackApp.Dto;
using FeedbackApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<DtoUserResponse>> GetUsers()
        {
            var users = _context.Users
                .Select(users => new DtoUserResponse
                {
                    Id = users.Id,
                    FullName = users.FullName,
                    Nickname = users.Nickname,
                    Email = users.Email,
                    Password = users.Password,
                    ProfileImageUrl = users.ProfileImageUrl
                })
                .ToList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<DtoUserResponse> GetUser(int id)
        {
            var users = _context.Users.FirstOrDefault(x => x.Id == id);

            if (users is null)
                return NotFound();

            var response = new DtoUserResponse
            {
                Id = users.Id,
                FullName = users.FullName,
                Nickname = users.Nickname,
                Email = users.Email,
                Password = users.Password,
                ProfileImageUrl = users.ProfileImageUrl

            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<DtoUserResponse>> AddUser([FromForm] DtoUserRequest userRequest)
        {
            // Model doğrulama
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Geçersiz model verisi.", Errors = ModelState });
            }

            var user = new User
            {
                FullName = userRequest.FullName,
                Nickname = userRequest.Nickname,
                Email = userRequest.Email,
                Password = userRequest.Password 
            };

            if (userRequest.ProfileImage != null && userRequest.ProfileImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userRequest.ProfileImage.FileName);
                var filePath = Path.Combine("wwwroot/images/profiles", fileName);

                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await userRequest.ProfileImage.CopyToAsync(fileStream);
                    }

                    user.ProfileImageUrl = $"/images/profiles/{fileName}"; 
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Resim kaydedilirken bir hata oluştu.", Details = ex.Message });
                }
            }

            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Kullanıcı kaydedilirken bir hata oluştu.", Details = ex.InnerException?.Message });
            }

            var response = new DtoUserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Nickname = user.Nickname,
                Email = user.Email,
                Password = user.Password,
                ProfileImageUrl = user.ProfileImageUrl 
            };

            return Ok(response); 
        }

        [HttpDelete("{id}")]
        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user is null)
                return false;
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
