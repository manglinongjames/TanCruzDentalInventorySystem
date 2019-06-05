using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagement.IdentityStore
{
    public class ApplicationGroupManager<TGroup, TKey> :
        IDisposable
    {
        private readonly IGroupStore<TGroup, string> _groupStore;

        public ApplicationGroupManager(IGroupStore<TGroup, string> store)
        {
            _groupStore = store;
        }

        public IIdentityValidator<TGroup> RoleValidator { get; set; }

        public virtual IQueryable<TGroup> Groups
        {
            get
            {
                return _groupStore.Groups.AsQueryable();
            }
        }

        public virtual Task<IdentityResult> CreateAsync(TGroup group)
        {
            return null;
        }

        public virtual Task<IdentityResult> DeleteAsync(TGroup group)
        {
            return null;
        }
        
        public virtual Task<TGroup> FindByIdAsync(TKey groupId)
        {
            return null;
        }
        
        public virtual Task<TGroup> FindByNameAsync(string groupName)
        {
            return null;
        }
        
        public virtual Task<bool> GroupExistsAsync(string roleName)
        {
            return null;
        }

        public virtual Task<IdentityResult> UpdateAsync(TGroup group)
        {
            return null;
        }

        public void Dispose()
        {

        }

        protected virtual void Dispose(bool disposing)
        {

        }
    }
}
