using AuthExample.Auth.Db;
using AuthExample.Auth.Db.Entities;
using System.Collections.Generic;

namespace AuthExample.Auth
{
    /// <summary>
    /// TODO: add permission
    /// </summary>
    internal class UserService : IUserService
    {
        private readonly IDbRepository _dbRepository;

        public UserService(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public virtual List<User> Get()
        {
            var users = _dbRepository.GetUsers();

            return users;
        }

        public virtual User Get(string id)
        {
            var user = _dbRepository.GetUser(id);

            return user;
        }

        public virtual User GetByEmail(string email)
        {
            var user = _dbRepository.GetUserByEmail(email);

            return user;
        }

        public virtual void Update(User user, string actionNote = null)
        {
            var actualUser = Get(user.Id);

            if (user != actualUser)
            {
                _dbRepository.Update(user, actionNote);
            }
        }

        public virtual void Delete(string id, string actionNote = null)
        {
            var user = Get(id);

            if (user is not null)
            {
                _dbRepository.DeleteUser(id, actionNote);
            }
        }

        public virtual bool MatchPassword(User user, string password)
        {
            var currentPassword = _dbRepository.GetPassword(user.Id);
            var isMatch = currentPassword == password;

            return isMatch;
        }
    }
}
