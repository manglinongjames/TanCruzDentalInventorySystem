using System;
using System.Security.Cryptography;
using Dapper;
using TanCruzDentalInventorySystem.Models;
using TanCruzDentalInventorySystem.Repository.DataServiceInterface;

namespace TanCruzDentalInventorySystem.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private const string GET_USERPROFILE = "SELECT USER_NAME UserName, LAST_NAME LastName, PASSWORD Password FROM T1_USERPROFILE WHERE USER_NAME = @USER_NAME;";

        public IUnitOfWork UnitOfWork { get; set; }

        public UserProfile Login(string userName, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@USER_NAME", userName, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            var userProfile = UnitOfWork.Connection.QuerySingleOrDefault<UserProfile>(
                sql: GET_USERPROFILE,
                param: parameters,
                transaction: UnitOfWork.Transaction,
                commandType: System.Data.CommandType.Text);

            return userProfile;
        }
    }
}