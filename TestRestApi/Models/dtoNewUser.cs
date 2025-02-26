using System.ComponentModel.DataAnnotations;

namespace TestRestApi.Models
{
    public class dtoNewUser
    {
        
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }


    }
}
