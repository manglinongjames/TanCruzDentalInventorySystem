﻿using IdentityManagement.Data;
using IdentityManagement.Entities;
using System.Collections.Generic;

namespace IdentityManagement.Repositories
{
    public static class RoleRepository
    {
        public static int NewUserRole(string userID, string roleName)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "UserID", ParameterValue = userID });
            parameters.Add(new ParameterInfo() { ParameterName = "RoleName", ParameterValue = roleName });
            int success = SqlHelper.ExecuteQuery("NewUserRole", parameters);
            return success;
        }

        public static int DeleteUserRole(string userID, string roleName)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "UserID", ParameterValue = userID });
            parameters.Add(new ParameterInfo() { ParameterName = "RoleName", ParameterValue = roleName });
            int success = SqlHelper.ExecuteQuery("DeleteUserRole", parameters);
            return success;
        }

        public static IList<ApplicationRole> GetRoles()
        {
            IList<ApplicationRole> oRoleList = SqlHelper.GetRecords<ApplicationRole>("GetRoles", null);
            return oRoleList;
        }

        public static IList<string> GetUserRoles(string userID)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "USER_ID", ParameterValue = userID });
            IList<string> roles = SqlHelper.GetRecords<string>("GetUserRoles", parameters);
            return roles;
        }

        //public static UserInfo GetUserByUsername(string userName)
        //{
        //    List<ParameterInfo> parameters = new List<ParameterInfo>();
        //    parameters.Add(new ParameterInfo() { ParameterName = "Username", ParameterValue = userName });
        //    UserInfo oUser = SqlHelper.GetRecord<UserInfo>("GetUserByUsername", parameters);
        //    return oUser;
        //}
    }
}
