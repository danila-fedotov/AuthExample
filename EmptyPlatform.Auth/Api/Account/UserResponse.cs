using System;

namespace EmptyPlatform.Auth.Api.Account
{
    public class UserResponse
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime Birthday { get; set; }

        public static UserResponse Map(Db.User user) => new()
        {
            UserId = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            Birthday = user.Birthday
        };
    }
}
