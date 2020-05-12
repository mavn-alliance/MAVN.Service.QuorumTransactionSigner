using System.Threading.Tasks;
using JetBrains.Annotations;
using MAVN.Common.MsSql;
using MAVN.Service.QuorumTransactionSigner.Domain.Repositories;
using MAVN.Service.QuorumTransactionSigner.MsSqlRepositories.Contexts;
using MAVN.Service.QuorumTransactionSigner.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.QuorumTransactionSigner.MsSqlRepositories
{
    [UsedImplicitly]
    public class WalletRepository : IWalletRepository
    {
        private readonly MsSqlContextFactory<QtsContext> _contextFactory;

        public WalletRepository(
            MsSqlContextFactory<QtsContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<byte[]> GetPublicKeyAsync(string address)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.Wallets.FindAsync(address);
                return entity?.PublicKey;
            }
        }

        public async Task SaveWalletAsync(
            string address,
            byte[] publicKey)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = new WalletEntity
                {
                    Address = address,
                    PublicKey = publicKey
                };

                context.Wallets.Add(entity);
                
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> WalletExistsAsync(
            string address)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                return await context.Wallets
                    .AnyAsync(x => x.Address == address);
            }
        }
    }
}
