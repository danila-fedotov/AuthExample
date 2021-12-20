using System.ComponentModel.DataAnnotations;

namespace EmptyPlatform.Auth.Api
{
    public class SignInDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
