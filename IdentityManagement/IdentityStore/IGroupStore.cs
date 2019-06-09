using IdentityManagement.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagement.IdentityStore
{
	public interface IGroupStore<TGroup, in TKey> : IDisposable
	{
		Task CreateAsync(TGroup group);
		Task DeleteAsync(TGroup group);
		Task<TGroup> FindByIdAsync(string groupId);
		Task<TGroup> FindByNameAsync(string groupName);
		IQueryable<TGroup> GetUserGroups(string userId);
		Task UpdateAsync(TGroup group);
		IQueryable<TGroup> Groups { get; }
		Task RemoveUserFromGroupAsync(string userId, string groupId);
		Task RemoveRolesFromGroupAsync(string groupId, string roleId);
		Task AddUserToGroupAsync(string userId, string groupId);
		Task AddRoleToGroupAsync(string groupId, string roleId);
		Task<IQueryable<ApplicationRole>> GetGroupRoles(string groupId);
	}
}
