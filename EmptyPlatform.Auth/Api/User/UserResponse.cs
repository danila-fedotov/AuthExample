using System;

namespace EmptyPlatform.Auth.Api.User
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime Birthday { get; set; }

        public static UserResponse Map(Db.User user) => new()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            Birthday = user.Birthday
        };
    }
}
