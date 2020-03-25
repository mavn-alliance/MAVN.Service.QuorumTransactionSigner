using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.QuorumTransactionSigner.Client.Models.Requests;
using Lykke.Service.QuorumTransactionSigner.Client.Models.Responses;
using Refit;

namespace Lykke.Service.QuorumTransactionSigner.Client
{
    /// <summary>
    ///    QuorumTransactionSigner client API interface.
    /// </summary>
    [PublicAPI]
    public interface IQuorumTransactionSignerWalletsApi
    {
        /// <summary>
        ///    Create new wallet.
        /// </summary>
        /// <returns>
        ///    Create wallet response, which contains address of created wallet.
        /// </returns>
        [Post("/api/wallets")]
        Task<CreateWalletResponse> CreateWalletAsync();

        /// <summary>Sign transaction with a specified address' private key.</summary>
        /// <param name="address">Address of a signer (20 bytes) in a hex format (40 hexadecimal symbols) prefixed with 0x.</param>
        /// <param name="body">Request body, that contains raw transaction hash in the base64 encoding.</param>
        /// <returns>Sign transaction response, which contains signed transaction in a hex format.</returns>
        [Post("/api/wallets/{address}/sign")]
        Task<SignTransactionResponse> SignTransactionAsync(string address, [Body] SignTransactionRequest body);

        /// <summary>Sign transaction with a specified address' private key.</summary>
        /// <param name="address">Address of a signer (20 bytes) in a hex format (40 hexadecimal symbols) prefixed with 0x.</param>
        /// <param name="hashes">Request body, that contains raw transaction hash in the base64 encoding.</param>
        /// <returns>Sign transaction response, which contains signed transactions in a hex format.</returns>
        [Post("/api/wallets/{address}/batchsign")]
        Task<SignTransactionsBatchResponse> SignTransactionsBatchAsync(string address, [Body] List<string> hashes);
    }
}
