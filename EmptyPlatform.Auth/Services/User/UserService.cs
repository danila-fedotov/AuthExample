using EmptyPlatform.Auth.Db;
using System;
using System.Collections.Generic;
using System.Linq;

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
            user.UserId = Guid.NewGuid().ToString();

            _dbRepository.CreateUser(user, actionNote);

            // TODO: form link
        }

        public virtual User Get(string userId)
        {
            var user = _dbRepository.GetUserById(userId) ?? throw new ArgumentNullException("UserId", "User is not found");
            var roles = _dbRepository.GetRolesByUserId(userId);

            user.Roles = roles;

            return user;
        }

        public virtual User GetByEmail(string email)
        {
            var user = _dbRepository.GetUserByEmail(email) ?? throw new ArgumentNullException("Email", "User is not found");

            return user;
        }

        public virtual List<User> Get()
        {
            var users = _dbRepository.GetUsers();

            return users;
        }

        public virtual void Update(User user, string actionNote)
        {
            var actualUser = Get(user.UserId);

            if (!actualUser.Equals(user))
            {
                _dbRepository.UpdateUser(user, actionNote);
            }

            // TODO: email change
        }

        public virtual void Remove(string userId, string actionNote)
        {
            _dbRepository.RemoveUser(userId, actionNote);
        }

        public virtual bool MatchPassword(string userId, string password)
        {
            var actualPassword = _dbRepository.GetPassword(userId);
            var isMatched = actualPassword == password;

            return isMatched;
        }

        public virtual bool ValidateAccess(Dictionary<string, string[]> Permissions, string controllerName, string actionName)
        {
            try
            {
                var isAccountController = controllerName == "account";

                if (isAccountController)
                {
                    return true;
                }

                var hasAccessToController = Permissions.TryGetValue(controllerName, out string[] actions);

                if (!hasAccessToController)
                {
                    return false;
                }

                var hasAccessToAllActions = !actions.Any();
                var hasAccessToAction = hasAccessToAllActions || actions.Contains(actionName);

                return hasAccessToAction;
            }
            catch (Exception ex)
            {
                // TODO: log
                return false;
            }
        }
    }
}
