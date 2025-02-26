using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TestRestApi.DATA.Models;

namespace TestRestApi.DATA
{
    public class APPDbContext  : IdentityDbContext<UserData>
    {

        public APPDbContext(DbContextOptions<APPDbContext> options) : base(options) 
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }






    }
}
