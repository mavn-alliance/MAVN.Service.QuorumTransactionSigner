using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.QuorumTransactionSigner.Client.Exceptions;
using Lykke.Service.QuorumTransactionSigner.Client.Models.Requests;
using Lykke.Service.QuorumTransactionSigner.Client.Models.Responses;

namespace Lykke.Service.QuorumTransactionSigner.Client
{
    /// <summary>
    ///    Extension methods for <see cref="IQuorumTransactionSignerClient"/>.
    /// </summary>
    [PublicAPI]
    public static class QuorumTransactionSignerClientExtensions
    {
        /// <summary>Create new wallet.</summary>
        /// <returns>New wallet address.</returns>
        public static async Task<string> CreateWalletAsync(
            [NotNull] this IQuorumTransactionSignerClient client)
        {
            var response =  await client.WalletsApi.CreateWalletAsync();

            return response.Address;
        }

        /// <summary>Sign raw transaction hash by a specified wallet.</summary>
        /// <param name="client"><see cref="IQuorumTransactionSignerClient"/> instance.</param>
        /// <param name="address">Wallet address (20 bytes) in a hex format (40 hexadecimal symbols) prefixed with 0x.</param>
        /// <param name="rawTxHash">Raw transaction hash in the base64 encoding.</param>
        /// <returns>V, R and S parameters of the transaction signature.</returns>
        /// <exception cref="WalletNotFoundException">Throw, if a wallet for the specified address has not been found.</exception>
        public static async Task<(byte[] V, byte[] R, byte[] S)> SignTransactionAsync(
            [NotNull] this IQuorumTransactionSignerClient client,
            [NotNull] string address,
            [NotNull] byte[] rawTxHash)
        {
            var request = new SignTransactionRequest { RawTxHash = Convert.ToBase64String(rawTxHash) };
            var response = await client.WalletsApi.SignTransactionAsync(address, request);

            if (response.Error != SignTransactionError.None)
                throw new WalletNotFoundException(address);

            var r = Convert.FromBase64String(response.R);
            var s = Convert.FromBase64String(response.S);
            var v = Convert.FromBase64String(response.V);

            return (v, r, s);
        }

        /// <summary>Sign raw transaction hash by a specified wallet.</summary>
        /// <param name="client"><see cref="IQuorumTransactionSignerClient"/> instance.</param>
        /// <param name="address">Wallet address (20 bytes) in a hex format (40 hexadecimal symbols) prefixed with 0x.</param>
        /// <param name="rawTxHashes">Raw transaction hashes in the base64 encoding.</param>
        /// <returns>V, R and S parameters of the transaction signature.</returns>
        /// <exception cref="WalletNotFoundException">Throw, if a wallet for the specified address has not been found.</exception>
        public static async Task<List<(byte[] V, byte[] R, byte[] S)>> SignTransactionsAsync(
            [NotNull] this IQuorumTransactionSignerClient client,
            [NotNull] string address,
            [NotNull] List<byte[]> rawTxHashes)
        {
            var hashes = rawTxHashes.Select(Convert.ToBase64String).ToList();
            var response = await client.WalletsApi.SignTransactionsBatchAsync(address, hashes);

            if (response.Error != SignTransactionError.None)
                throw new WalletNotFoundException(address);

            var result = new List<(byte[], byte[], byte[])>(hashes.Count);

            foreach (var hash in hashes)
            {
                var signedData = response.HashToSignedTxDict[hash];
                var r = Convert.FromBase64String(signedData.R);
                var s = Convert.FromBase64String(signedData.S);
                var v = Convert.FromBase64String(signedData.V);
                result.Add((v, r, s));
            }

            return result;
        }
    }
}
