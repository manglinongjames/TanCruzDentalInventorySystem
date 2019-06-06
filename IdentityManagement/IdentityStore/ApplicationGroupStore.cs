using IdentityManagement.Entities;
using IdentityManagement.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagement.IdentityStore
{
	public class ApplicationGroupStore :
		IGroupStore<ApplicationGroup, string>
	{
		public IQueryable<ApplicationGroup> Groups
		{
			get
			{
				return GroupRepository.GetGroups().AsQueryable();
			}
		}

		public Task CreateAsync(ApplicationGroup group)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(ApplicationGroup group)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationGroup> FindByIdAsync(string groupId)
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationGroup> FindByNameAsync(string groupName)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(ApplicationGroup group)
		{
			throw new NotImplementedException();
		}

		public IQueryable<ApplicationGroup> GetUserGroups(string userId)
		{
			IQueryable<ApplicationGroup> groups = GroupRepository.GetUserGroups(userId).AsQueryable();
			return groups;
		}
		public Task RemoveUserFromGroupAsync(string userId, string groupId)
		{
			return Task.Factory.StartNew(() =>
			{
				GroupRepository.RemoveUserFromGroup(userId, groupId);
				return;
			});
		}

		public Task AddUserToGroupAsync(string userId, string groupId)
		{
			return Task.Factory.StartNew(() =>
			{
				GroupRepository.AddUserToGroup(userId, groupId);
				return;
			});
		}
	}
}
