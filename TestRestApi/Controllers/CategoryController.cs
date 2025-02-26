using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRestApi.DATA;
using TestRestApi.DATA.Models;

namespace TestRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public CategoryController(APPDbContext db)
        {
            _db = db;
        }

        private readonly APPDbContext _db;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneCategory(int id)
        {
            var x = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id );
            if (x == null)
            {
                return NotFound("item not found Xoxo");
            }
            return Ok(x);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategorires(int id)
        {
            var cats = await _db.Categories.ToListAsync();

            return Ok(cats);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategories(String category, string description)
        {
            Category c = new() { Name = category, Description = description };
            await _db.Categories.AddAsync(c);
            _db.SaveChanges();
            return Ok(c);
        }
        [HttpPut]
        public async Task<IActionResult> updateCategory(Category category)
        {
            var x = await _db.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            if (x == null)
            {
                return BadRequest("not found");
            }
            x.Name = category.Name;
            x.Description = category.Description;
            _db.SaveChanges();
            return Ok(x);
        }
        [HttpDelete("id")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            var x = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (id == 0)
            {
                return BadRequest("not found");

            }
            _db.Categories.Remove(x);
            _db.SaveChanges();
            return Ok(x);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult>
            updateCategoryPatch([FromBody] JsonPatchDocument<Category> Category, [FromRoute] int id)
        {
            var x = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (x == null)
            {
                return BadRequest("not found");
            }
            Category.ApplyTo(x);
            await _db.SaveChangesAsync();
            return Ok(x);
        }
      





    }
}
