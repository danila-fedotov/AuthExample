﻿using EmptyPlatform.Auth.Db;
using System;
using System.Collections.Generic;

namespace EmptyPlatform.Auth.Services
{
    internal class RoleService : IRoleService
    {
        private readonly IDbRepository _dbRepository;

        public RoleService(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public virtual Role Get(string roleId)
        {
            var role = _dbRepository.GetRoleById(roleId) ?? throw new ArgumentNullException("RoleId", "Role is not found");

            return role;
        }

        public virtual List<Role> Get()
        {
            var roles = _dbRepository.GetRoles();

            return roles;
        }

        public virtual void Update(Role role, string actionNote)
        {
            var actualRole = Get(role.RoleId);

            if (actualRole.Name != role.Name)
            {
                _dbRepository.UpdateRole(role, actionNote);
            }

            if (actualRole.PermissionsAsJson != role.PermissionsAsJson)
            {
                _dbRepository.UpdateRolePermissions(role.RoleId, role.PermissionsAsJson, actionNote);
            }
        }
    }
}
