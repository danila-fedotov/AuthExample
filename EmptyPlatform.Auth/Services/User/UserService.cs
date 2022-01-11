using EmptyPlatform.Auth.Db;
using System;
using System.Collections.Generic;

namespace EmptyPlatform.Auth.Services
{
    /// <summary>
    /// TODO: hash password
    /// TOOD: create password
    /// </summary>
    internal class UserService : IUserService
    {
        private readonly IDbRepository _dbRepository;

        public UserService(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public void Create(User user, string actionNote)
        {
            var hasId = !string.IsNullOrEmpty(user.Id);

            if (hasId)
            {
                var actualUser = Get(user.Id);

                if (actualUser is not null)
                {
                    throw new ApplicationException($"User already created");
                }
            }

            user.Id = Guid.NewGuid().ToString();

            _dbRepository.CreateUser(user, actionNote);

            // TODO: form link
        }

        public virtual List<User> Get()
        {
            var users = _dbRepository.GetUsers();

            return users;
        }

        public virtual User Get(string userId)
        {
            var user = _dbRepository.GetUserById(userId);

            if (user is not null)
            {
                var roles = _dbRepository.GetRolesByUserId(userId);

                user.Roles = roles;
            }

            return user;
        }

        public virtual User GetByEmail(string email)
        {
            var user = _dbRepository.GetUserByEmail(email);

            return user;
        }

        public virtual void Update(User user, string actionNote)
        {
            var actualUser = Get(user.Id);

            if (user != actualUser)
            {
                _dbRepository.UpdateUser(user, actionNote);
            }
        }

        public virtual void Remove(User user, string actionNote)
        {
            _dbRepository.RemoveUser(user.Id, actionNote);
        }

        public virtual bool MatchPassword(User user, string password)
        {
            var currentPassword = _dbRepository.GetPassword(user.Id);
            var isMatch = currentPassword == password;

            return isMatch;
        }
    }
}
