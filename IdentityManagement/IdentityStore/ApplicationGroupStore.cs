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
			return Task.Factory.StartNew(() =>
			{
				GroupRepository.CreateNewGroup(group);
			});
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
			return Task.Factory.StartNew(() =>
			{
				var group = GroupRepository.GetGroupById(groupId);
				return group;
			});
		}

		public Task<ApplicationGroup> FindByNameAsync(string groupId)
		{
			return Task.Factory.StartNew(() =>
			{
				var group = GroupRepository.GetGroupByName(groupId);
				return group;
			});
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

		public Task RemoveRoleFromGroupAsync(string groupId, string roleId)
		{
			return Task.Factory.StartNew(() =>
			{
				GroupRepository.RemoveRoleFromGroup(groupId, roleId);
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

		public Task AddRoleToGroupAsync(string groupId, string roleId)
		{
			return Task.Factory.StartNew(() =>
			{
				GroupRepository.AddRoleToGroup(groupId, roleId);
				return;
			});
		}

		public Task<IQueryable<ApplicationRole>> GetGroupRoles(string groupId)
		{
			return Task.Factory.StartNew(() =>
			{
				var roles = GroupRepository.GetGroupRoles(groupId).AsQueryable();
				return roles;
			});
		}
	}
}
