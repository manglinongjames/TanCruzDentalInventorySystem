using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagement.IdentityStore
{
	public interface IGroupStore<TGroup, in TKey> : IDisposable
	{
		Task CreateAsync(TGroup group);
		Task DeleteAsync(TGroup group);
		Task<TGroup> FindByIdAsync(TKey groupId);
		Task<TGroup> FindByNameAsync(string groupName);
		IQueryable<TGroup> GetUserGroups(string userId);
		Task UpdateAsync(TGroup group);
		IQueryable<TGroup> Groups { get; }
		Task RemoveUserFromGroupAsync(string userId, string groupId);
		Task AddUserToGroupAsync(string userId, string groupId);
	}
}
