using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAVN.Service.QuorumTransactionSigner.Domain.Services
{
    public interface IWalletService
    {
        Task<string> CreateWalletAsync();

        Task<(byte[] V, byte[] R, byte[] S)> SignTransactionAsync(string address, byte[] rawTxHash);

        Task<Dictionary<string, (byte[] V, byte[] R, byte[] S)>> SignTransactionsAsync(
            string address, Dictionary<string, byte[]> rawTxHashes);

        Task<bool> WalletExistsAsync(string address);
    }
}
