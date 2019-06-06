using IdentityManagement.Data;
using IdentityManagement.Entities;
using System.Collections.Generic;

namespace IdentityManagement.Repositories
{
	public class GroupRepository
	{
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

		public static void AddUserToGroup(string userId, string groupId)
		{
			List<ParameterInfo> parameters = new List<ParameterInfo>();
			parameters.Add(new ParameterInfo() { ParameterName = "USER_ID", ParameterValue = userId });
			parameters.Add(new ParameterInfo() { ParameterName = "GROUP_ID", ParameterValue = groupId });
			SqlHelper.ExecuteQuery("AddUserToGroup", parameters);
			return;
		}

	}
}
