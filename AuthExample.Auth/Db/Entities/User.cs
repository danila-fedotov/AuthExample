using System;

namespace AuthExample.Auth.Db.Entities
{
    public class User
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime Birthday { get; set; }

        public override bool Equals(object obj)
        {
            var anotherUser = obj as User;

            if (anotherUser is null) return false;

            return Id == anotherUser.Id &&
                Email == anotherUser.Email &&
                FirstName == anotherUser.FirstName &&
                SecondName == anotherUser.SecondName &&
                Birthday == anotherUser.Birthday;
        }

        public string FullName => $"{FirstName} {SecondName}";
    }
}
