using System.Data.Common;
using MAVN.Persistence.PostgreSQL.Legacy;
using MAVN.Service.QuorumTransactionSigner.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.QuorumTransactionSigner.MsSqlRepositories.Contexts
{
    public class QtsContext : PostgreSQLContext
    {
        private const string Schema = "quorum_transaction_signer";
        
        public QtsContext()
            : base(Schema)
        {
        }

        public QtsContext(
            string connectionString,
            bool isTraceEnabled)
            : base(Schema, connectionString, isTraceEnabled)
        {
        }

        public QtsContext(DbConnection dbConnection)
            : base(Schema, dbConnection)
        {
        }

        internal DbSet<WalletEntity> Wallets { get; set; }

        protected override void OnMAVNModelCreating(
            ModelBuilder modelBuilder)
        {
        }
    }
}
