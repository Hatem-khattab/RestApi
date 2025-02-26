namespace TestRestApi.Models
{
    public class mdlitemsClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; }
        public bool IsAvailable { get; set; }
        public string Notes { get; set; }

        public int CategoryId { get; set; }







    }
}
