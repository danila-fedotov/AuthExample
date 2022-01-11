using System;
using System.ComponentModel.DataAnnotations;

namespace EmptyPlatform.Auth.Api.User
{
    public class UpdateRequest
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        public string SecondName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public string ActionNote { get; set; }

        public Db.User Map() => new()
        {
            Id = Id,
            Email = Email,
            FirstName = FirstName,
            SecondName = SecondName,
            Birthday = Birthday
        };
    }
}
