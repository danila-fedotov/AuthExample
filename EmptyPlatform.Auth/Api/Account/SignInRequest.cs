using System.ComponentModel.DataAnnotations;

namespace EmptyPlatform.Auth.Api.Account
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
