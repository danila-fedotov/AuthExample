using System.ComponentModel.DataAnnotations;

namespace EmptyPlatform.Auth.Api
{
    public class RemoveDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ActionNote { get; set; }
    }
}
