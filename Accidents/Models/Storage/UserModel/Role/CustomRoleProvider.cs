using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Accidents.Models.Storage.UserModel
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] roles = new string[] { };

            using (DatabaseContext db = new DatabaseContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Email == username);

                if (user != null)
                {
                    Role role = db.Roles.Find(user.RoleId);

                    if (role != null) 
                        roles = new string[] { role.Name };
                }
            }

            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            string[] roles = new string[] { };

            using (DatabaseContext db = new DatabaseContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Email == username);

                if (user != null)
                {
                    Role role = db.Roles.Find(user.RoleId);

                    if (role != null && role.Name == roleName) return true;
                }
            }

            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}