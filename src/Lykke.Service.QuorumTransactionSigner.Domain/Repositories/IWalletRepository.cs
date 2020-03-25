using System.Threading.Tasks;

namespace Lykke.Service.QuorumTransactionSigner.Domain.Repositories
{
    public interface IWalletRepository
    {
        Task<byte[]> GetPublicKeyAsync(
            string address);
        
        Task SaveWalletAsync(
            string address,
            byte[] publicKey);

        Task<bool> WalletExistsAsync(
            string address);
    }
}
