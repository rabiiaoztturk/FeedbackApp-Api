using FeedbackApp.Data;
using FeedbackApp.Dto;
using FeedbackApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<DtoCategoryResponse>> GetCategories()
        {
            var categories = _context.Categories
                .Select(categories => new DtoCategoryResponse
                {
                    Id = categories.Id,
                    Name = categories.Name
                })
                .ToList();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public ActionResult<DtoCategoryResponse> GetCategory(int id)
        {
            var categories = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (categories is null)
                return NotFound();

            var response = new DtoCategoryResponse
            {
                Id = categories.Id,
                Name = categories.Name

            };

            return Ok(response);
        }

        [HttpPost]
        public ActionResult<Category> AddCategory([FromBody] DtoCategoryRequest categoryRequest)
        {
            var categories = new Category
            {
                Name = categoryRequest.Name
            };

            _context.Categories.Add(categories);
            _context.SaveChanges();

            var response = new DtoCategoryResponse
            {
                Id = categories.Id,
                Name = categories.Name
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public bool DeleteCategory(int id)
        {
            var categories = _context.Categories.Find(id);
            if (categories is null)
                return false;
            _context.Categories.Remove(categories);
            _context.SaveChanges();
            return true;
        }
    }
}
