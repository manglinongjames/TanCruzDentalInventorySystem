using System;
using System.Security.Cryptography;
using Dapper;
using TanCruzDentalInventorySystem.Models;
using TanCruzDentalInventorySystem.Repository.DataServiceInterface;

namespace TanCruzDentalInventorySystem.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private const string GET_USERPROFILE = "[dbo].InSys_GetUserProfile";

        public IUnitOfWork UnitOfWork { get; set; }

        public bool Login(string userName, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@USER_NAME", userName, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            var userProfile = UnitOfWork.Connection.QuerySingleOrDefault<UserProfile>(
                sql: GET_USERPROFILE,
                param: parameters,
                transaction: UnitOfWork.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);

            return new PowerPasswordHasher(Convert.FromBase64String(userProfile.Password)).Verify(password);
        }
    }

    internal sealed class PowerPasswordHasher
    {
        const int SaltSize = 16, HashSize = 20, HashIter = 10000;
        readonly byte[] _salt, _hash;

        public PowerPasswordHasher(string password)
        {
            new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
            _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
        }

        public PowerPasswordHasher(byte[] hashBytes)
        {
            Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
        }

        public byte[] ToArray()
        {
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
            return hashBytes;
        }

        public bool Verify(string password)
        {
            byte[] test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
            for (int i = 0; i < HashSize; i++)
                if (test[i] != _hash[i])
                    return false;
            return true;
        }
    }
}