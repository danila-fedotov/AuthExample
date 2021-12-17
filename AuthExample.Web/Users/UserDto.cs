using AuthExample.Auth.Db.Entities;
using System;

namespace AuthExample.Web.Users
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime Birthday { get; set; }

        public User UserEntity => new()
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
