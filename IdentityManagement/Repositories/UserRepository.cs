using IdentityManagement.Data;
using IdentityManagement.Entities;
using System.Collections.Generic;

namespace IdentityManagement.Repositories
{
    public static class UserRepository
    {
        public static int CreateNewUser(ApplicationUser objUser)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "UserName", ParameterValue = objUser.UserName });
            parameters.Add(new ParameterInfo() { ParameterName = "FirstName", ParameterValue = objUser.FirstName });
            parameters.Add(new ParameterInfo() { ParameterName = "LastName", ParameterValue = objUser.LastName });
            parameters.Add(new ParameterInfo() { ParameterName = "MiddleName", ParameterValue = objUser.MiddleName });
            parameters.Add(new ParameterInfo() { ParameterName = "Email", ParameterValue = objUser.Email });
            parameters.Add(new ParameterInfo() { ParameterName = "Password", ParameterValue = objUser.Password });
            parameters.Add(new ParameterInfo() { ParameterName = "UserStatus", ParameterValue = objUser.UserStatus });
            int success = SqlHelper.ExecuteQuery("CreateNewUser", parameters);
            return success;
        }

        public static int DeleteUser(ApplicationUser objUser)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Id", ParameterValue = objUser.Id });
            int success = SqlHelper.ExecuteQuery("DeleteUser", parameters);
            return success;
        }

        public static ApplicationUser GetUser(string id)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "USER_ID", ParameterValue = id });
            ApplicationUser oUser = SqlHelper.GetRecord<ApplicationUser>("GetUserProfile", parameters);
            return oUser;
        }

        public static IList<ApplicationUser> GetUsers()
        {
            IList<ApplicationUser> oUserList = SqlHelper.GetRecords<ApplicationUser>("GetUsers", null);
            return oUserList;
        }

        public static ApplicationUser GetUserByUsername(string userName)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "USER_NAME", ParameterValue = userName });
            ApplicationUser oUser = SqlHelper.GetRecord<ApplicationUser>("GetUserProfileByUserName", parameters);
            return oUser;
        }

        public static int UpdateUser(ApplicationUser objUser)
        {
			List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "UserId", ParameterValue = objUser.UserId });
            parameters.Add(new ParameterInfo() { ParameterName = "UserName", ParameterValue = objUser.UserName });
            parameters.Add(new ParameterInfo() { ParameterName = "FirstName", ParameterValue = objUser.FirstName });
            parameters.Add(new ParameterInfo() { ParameterName = "LastName", ParameterValue = objUser.LastName });
            parameters.Add(new ParameterInfo() { ParameterName = "MiddleName", ParameterValue = objUser.MiddleName });
            parameters.Add(new ParameterInfo() { ParameterName = "Email", ParameterValue = objUser.Email });
			parameters.Add(new ParameterInfo() { ParameterName = "Password", ParameterValue = objUser.Password });
			parameters.Add(new ParameterInfo() { ParameterName = "UserStatus", ParameterValue = objUser.UserStatus });
			int success = SqlHelper.ExecuteQuery("UpdateUserProfile", parameters);
            return success;
        }
    }
}
