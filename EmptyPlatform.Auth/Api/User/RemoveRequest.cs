using System.ComponentModel.DataAnnotations;

namespace EmptyPlatform.Auth.Api.User
{
    public class RemoveRequest
    {
        [Required]
        public string UserId { get; set; }

        public string ActionNote { get; set; }
    }
}
