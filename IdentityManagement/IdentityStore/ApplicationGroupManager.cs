using IdentityManagement.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagement.IdentityStore
{
	public class ApplicationGroupManager<TGroup> :
		IDisposable
	{
		private readonly IGroupStore<ApplicationGroup> _groupStore;

		public ApplicationGroupManager(IGroupStore<ApplicationGroup> store)
		{
			_groupStore = store;
		}

		public virtual IQueryable<ApplicationGroup> Groups
		{
			get
			{
				return _groupStore.Groups;
			}
		}

		public async virtual Task<IdentityResult> CreateAsync(ApplicationGroup group)
		{
			var existingGroup = await _groupStore.FindByNameAsync(group.GroupName);
			if (existingGroup != null)
				return IdentityResult.Failed(new string[] { $"GroupName {group.GroupName} already exists" });

			await _groupStore.CreateAsync(group);

			return IdentityResult.Success;
		}

		public async virtual Task<IdentityResult> DeleteAsync(ApplicationGroup group)
		{
			await _groupStore.DeleteAsync(group);
			return IdentityResult.Success;
		}

		public virtual async Task<ApplicationGroup> FindByIdAsync(string groupId)
		{
			var group = await _groupStore.FindByIdAsync(groupId);
			return group;
		}

		public virtual Task<IQueryable<ApplicationRole>> GetGroupRoles(string groupId)
		{
			var roles = _groupStore.GetGroupRoles(groupId);
			return roles;
		}

		public async virtual Task<ApplicationGroup> FindByNameAsync(string groupName)
		{
			var group = await _groupStore.FindByNameAsync(groupName);
			return group;
		}

		public virtual Task<bool> GroupExistsAsync(string groupName)
		{
			return null;
		}

		public virtual async Task<IdentityResult> UpdateAsync(ApplicationGroup group)
		{
			var thisGroup = await _groupStore.FindByIdAsync(group.GroupId);
			var existingGroup = await _groupStore.FindByNameAsync(group.GroupName);
			if (existingGroup != null && thisGroup.GroupId != existingGroup.GroupId)
				return IdentityResult.Failed(new string[] { $"GroupName {group.GroupName} already exists" });

			await _groupStore.UpdateAsync(group);

			return IdentityResult.Success;
		}

		public IQueryable<ApplicationGroup> GetUserGroups(string userId)
		{
			IQueryable<ApplicationGroup> groups = _groupStore.GetUserGroups(userId);
			return groups;
		}

		public virtual async Task<IdentityResult> RemoveUserFromGroupAsync(string userId, IEnumerable<string> groupIds)
		{
			foreach (var groupId in groupIds)
			{
				await _groupStore.RemoveUserFromGroupAsync(userId, groupId);
			}

			return IdentityResult.Success;
		}

		public virtual async Task<IdentityResult> RemoveRolesFromGroupAsync(string groupId, IEnumerable<string> roleIds)
		{
			foreach (var roleId in roleIds)
			{
				await _groupStore.RemoveRolesFromGroupAsync(groupId, roleId);
			}

			return IdentityResult.Success;
		}

		public virtual async Task<IdentityResult> AddUserToGroup(string userId, IEnumerable<string> groupIds)
		{
			foreach (var groupId in groupIds)
			{
				await _groupStore.AddUserToGroupAsync(userId, groupId);
			}

			return IdentityResult.Success;
		}

		public virtual async Task<IdentityResult> AddRoleToGroup(string groupId, IEnumerable<string> roleIds)
		{
			foreach (var roleId in roleIds)
			{
				await _groupStore.AddRoleToGroupAsync(groupId, roleId);
			}

			return IdentityResult.Success;
		}

		public void Dispose()
		{

		}

		protected virtual void Dispose(bool disposing)
		{

		}
	}
}
