﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EmptyPlatform.Auth.Db
{
    public class User
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime Birthday { get; set; }

        public List<Role> Roles { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not User anotherUser)
            {
                return false;
            }

            var isEqual = UserId == anotherUser.UserId
                && Email == anotherUser.Email
                && FirstName == anotherUser.FirstName
                && SecondName == anotherUser.SecondName
                && Birthday == anotherUser.Birthday;

            return isEqual;
        }

        public override int GetHashCode()
        {
            var hash = HashCode.Combine(UserId, Email);

            return hash;
        }

        public string FullName => $"{FirstName} {SecondName}";

        // todo: merge permissions
        public Dictionary<string, string[]> Permissions => Roles?.First().Permissions ?? new();
    }
}
