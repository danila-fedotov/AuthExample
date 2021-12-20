using EmptyPlatform.Auth.Db;
using System;
using System.ComponentModel.DataAnnotations;

namespace EmptyPlatform.Auth.Api
{
    public class UserDto
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

        public User Map() => new()
        {
            Id = Id,
            Email = Email,
            FirstName = FirstName,
            SecondName = SecondName,
            Birthday = Birthday
        };

        public static UserDto Map(User user) => new()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            Birthday = user.Birthday
        };
    }
}
