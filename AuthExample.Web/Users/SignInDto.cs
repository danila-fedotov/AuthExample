using System.ComponentModel.DataAnnotations;

namespace AuthExample.Web.Users
{
    public class SignInDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
