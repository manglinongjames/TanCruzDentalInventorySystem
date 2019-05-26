using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TanCruzDentalInventorySystem.Repository.DataServiceInterface;

namespace TanCruzDentalInventorySystem.Repository
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
            Id = Guid.NewGuid();
            Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString);
        }

        public IDbConnection Connection { get; set;  }
        public IDbTransaction Transaction { get; set; }

        public Guid Id { get; }

        public void Begin()
        {
            Connection.Open();
            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            Transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            if (Transaction != null)
                Transaction.Dispose();
            Transaction = null;

            if (Connection != null)
                Connection.Dispose();
        }
    }
}