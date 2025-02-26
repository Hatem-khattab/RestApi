using System.ComponentModel.DataAnnotations;

namespace TestRestApi.DATA.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public string? Notes { get; set; } // Changed to PascalCase

        public List<Item> Items { get; set; } = new(); // Avoid null reference issues

    }
}
