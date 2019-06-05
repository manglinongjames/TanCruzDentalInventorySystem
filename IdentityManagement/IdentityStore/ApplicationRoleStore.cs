using IdentityManagement.Entities;
using IdentityManagement.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagement.IdentityStore
{
    public class ApplicationRoleStore :
        IRoleStore<ApplicationRole>,
        IQueryableRoleStore<ApplicationRole>
    {
        #region IQueryableRoleStore
        public IQueryable<ApplicationRole> Roles
        {
            get
            {
                return RoleRepository.GetRoles().AsQueryable();
            }
        }
        #endregion

        #region IRoleStore
        public Task CreateAsync(ApplicationRole role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationRole role)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public Task<ApplicationRole> FindByIdAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRole> FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationRole role)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
