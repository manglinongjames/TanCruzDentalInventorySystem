using IdentityManagement.Data;
using IdentityManagement.Entities;
using System.Collections.Generic;

namespace IdentityManagement.Repositories
{
	public class GroupRepository
	{
		public static int CreateNewGroup(ApplicationGroup objGroup)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_NAME", ParameterValue = objGroup.GroupName });
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_DESC", ParameterValue = objGroup.GroupDescription });
			int success = SqlHelper.ExecuteQuery("CreateNewGroup", parameters);
			return success;
		}

		public static IList<ApplicationGroup> GetGroups()
		{
			IList<ApplicationGroup> oGroupList = SqlHelper.GetRecords<ApplicationGroup>("GetGroups", null);
			return oGroupList;
		}

		public static IList<ApplicationGroup> GetUserGroups(string userId)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "USER_ID", ParameterValue = userId });
			IList<ApplicationGroup> oUserGroupList = SqlHelper.GetRecords<ApplicationGroup>("GetUserGroups", parameters);
			return oUserGroupList;
		}

		public static void RemoveUserFromGroup(string userId, string groupId)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "USER_ID", ParameterValue = userId });
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_ID", ParameterValue = groupId });
			SqlHelper.ExecuteQuery("RemoveUserFromGroup", parameters);
			return;
		}

		public static void RemoveRoleFromGroup(string groupId, string roleId)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_ID", ParameterValue = groupId });
			parameters.Add(new ParameterInfo() { ParameterName = "ROLE_ID", ParameterValue = roleId });
			SqlHelper.ExecuteQuery("RemoveRoleFromGroup", parameters);
			return;
		}

		public static void AddUserToGroup(string userId, string groupId)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "USER_ID", ParameterValue = userId });
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_ID", ParameterValue = groupId });
			SqlHelper.ExecuteQuery("AddUserToGroup", parameters);
			return;
		}

		public static void AddRoleToGroup(string groupId, string roleId)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_ID", ParameterValue = groupId });
			parameters.Add(new ParameterInfo() { ParameterName = "ROLE_ID", ParameterValue = roleId });
			SqlHelper.ExecuteQuery("AddRoleToGroup", parameters);
			return;
		}

		public static ApplicationGroup GetGroupById(string groupId)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_ID", ParameterValue = groupId });
			ApplicationGroup oGroup = SqlHelper.GetRecord<ApplicationGroup>("GetGroupById", parameters);
			return oGroup;
		}

		public static ApplicationGroup GetGroupByName(string groupName)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_NAME", ParameterValue = groupName });
			ApplicationGroup oGroup = SqlHelper.GetRecord<ApplicationGroup>("GetGroupByName", parameters);
			return oGroup;
		}

		public static IList<ApplicationRole> GetGroupRoles(string groupId)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_ID", ParameterValue = groupId });
			IList<ApplicationRole> oUserGroupList = SqlHelper.GetRecords<ApplicationRole>("GetGroupRoles", parameters);
			return oUserGroupList;
		}

		public static int UpdateGroup(ApplicationGroup objGroup)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_ID", ParameterValue = objGroup.GroupId });
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_NAME", ParameterValue = objGroup.GroupName });
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_DESC", ParameterValue = objGroup.GroupDescription });
			int success = SqlHelper.ExecuteQuery("UpdateGroup", parameters);
			return success;
		}

		public static int DeleteGroup(ApplicationGroup objGroup)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_ID", ParameterValue = objGroup.GroupId });
			int success = SqlHelper.ExecuteQuery("DeleteGroup", parameters);
			return success;
		}
	}
}
