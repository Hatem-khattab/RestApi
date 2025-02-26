using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRestApi.DATA;
using TestRestApi.DATA.Models;
using TestRestApi.Models;
using static Azure.Core.HttpHeader;
using static System.Net.Mime.MediaTypeNames;

namespace TestRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {

        public ItemsController(APPDbContext db)
        {
            _db = db;

        }

        private readonly APPDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllItems(int id)
        {
            var items = await _db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if (items == null)
            {
                return NotFound("item is not found xp");
            }
            return Ok(items);
        }

        [HttpGet("itemswithcategory/{idcategory}")]
        public async Task<IActionResult> GetAllItemswithgategory(int idofcategory)
        {
            var items = await _db.Items.Where(x => x.CategoryId == idofcategory).ToListAsync();
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }



        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm] mdlitemsClass mdlitemsClass)
        {
            using var stream = new MemoryStream();
            await mdlitemsClass.Image.CopyToAsync(stream);


            var item = new Item
            {
                Name = mdlitemsClass.Name,
                Description = mdlitemsClass.Description,
                Price = mdlitemsClass.Price,
                IsAvailable = mdlitemsClass.IsAvailable,
                Notes = mdlitemsClass.Notes,
                CategoryId = mdlitemsClass.CategoryId,
                Image = stream.ToArray()

            };
            await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return Ok(item);

        }
        [HttpDelete("{id}")]

        public async Task<IActionResult> Deleteitem(int id)
        {
            var x = await _db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if (x == null)
            {
                return NotFound("the id is not related to any exisitng item");
            }
            _db.Items.Remove(x);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> updateitem(int id, [FromForm] mdlitemsClass mdl)
        {
            var x = await _db.Items.FindAsync(id);
            if (x == null)
            {
                return NotFound("not found");
            }
            var iscategoryexisit = await _db.Categories.AnyAsync(x => x.Id == mdl.CategoryId);
            if (iscategoryexisit == false)
            {
                return BadRequest("not found");
            }
            if (mdl.Image != null)
            {
                using var stream = new MemoryStream();
                await mdl.Image.CopyToAsync(stream);
            }
            x.Name = mdl.Name;
            x.Notes = mdl.Notes;
            x.Description = mdl.Description;
            x.CategoryId = mdl.CategoryId;
            x.Price = mdl.Price;
            x.IsAvailable = mdl.IsAvailable;
                
            _db.SaveChanges();
            return Ok();

        }




    }
}
