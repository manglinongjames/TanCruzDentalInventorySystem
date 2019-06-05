using IdentityManagement.Entities;
using IdentityManagement.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagement.IdentityStore
{
    public class UserStore :
        IUserStore<ApplicationUser>,
        IUserRoleStore<ApplicationUser>,
        IUserPasswordStore<ApplicationUser>,
        IQueryableUserStore<ApplicationUser>
    {

        #region IQueryableUserStore
        public IQueryable<ApplicationUser> Users
        {
            get
            {
                return UserRepository.GetUsers().AsQueryable();
            }
        }
        #endregion

        #region IUserStore
        public Task CreateAsync(ApplicationUser user)
        {
            if (user != null)
            {
                return Task.Factory.StartNew(() =>
                {
                    UserRepository.CreateNewUser(user);
                });
            }
            throw new ArgumentNullException("user");
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            if (user != null)
            {
                return Task.Factory.StartNew(() =>
                {
                    UserRepository.DeleteUser(user);
                });
            }
            throw new ArgumentNullException("user");
        }

        public void Dispose()
        {

        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return Task.Factory.StartNew(() =>
                {
                    return UserRepository.GetUser(userId);
                });
            }
            throw new ArgumentNullException("userId");
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                return Task.Factory.StartNew(() =>
                {
                    return UserRepository.GetUserByUsername(userName);
                });
            }
            throw new ArgumentNullException("userName");
        }

        public ApplicationUser FindByName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                return UserRepository.GetUserByUsername(userName);
            }
            throw new ArgumentNullException("userName");
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            if (user != null)
            {
                return Task.Factory.StartNew(() =>
                {
                    return UserRepository.UpdateUser(user);
                });
            }
            throw new ArgumentNullException("userName");
        }
        #endregion

        #region IUserRoleStore
        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            if (user != null)
            {
                return Task.Factory.StartNew(() =>
                {
                    RoleRepository.NewUserRole(user.Id, roleName);
                });
            }
            else
            {
                throw new ArgumentNullException("user");
            }
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            if (user != null)
            {
                return Task.Factory.StartNew(() =>
                {
                    RoleRepository.DeleteUserRole(user.Id, roleName);
                });
            }
            else
            {
                throw new ArgumentNullException("user");
            }
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            if (user != null)
            {
                return Task.Factory.StartNew(() =>
                {
                    IList<string> roles = RoleRepository.GetUserRoles(user.Id);
                    return roles;
                });
            }
            else
            {
                throw new ArgumentNullException("user");
            }
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            if (user != null)
            {
                return Task.Factory.StartNew(() =>
                {
                    IList<string> roles = RoleRepository.GetUserRoles(user.Id);
                    foreach (string role in roles)
                    {
                        if (role.ToUpper() == roleName.ToUpper())
                        {
                            return true;
                        }
                    }

                    return false;
                });
            }
            else
            {
                throw new ArgumentNullException("user");
            }
        }
        #endregion

        #region IUserPasswordStore
        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(string.IsNullOrEmpty(user.Password));
        }
        #endregion
    }
}
