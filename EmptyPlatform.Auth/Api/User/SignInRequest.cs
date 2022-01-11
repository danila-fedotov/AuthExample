using System.ComponentModel.DataAnnotations;

namespace EmptyPlatform.Auth.Api.User
{
    public class SignInRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
