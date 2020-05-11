using MAVN.Common.MsSql;
using MAVN.Service.QuorumTransactionSigner.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.QuorumTransactionSigner.MsSqlRepositories.Contexts
{
    public class QtsContext : MsSqlContext
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

        
        internal DbSet<WalletEntity> Wallets { get; set; }

        
        protected override void OnLykkeConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnLykkeModelCreating(
            ModelBuilder modelBuilder)
        {
        }
    }
}
