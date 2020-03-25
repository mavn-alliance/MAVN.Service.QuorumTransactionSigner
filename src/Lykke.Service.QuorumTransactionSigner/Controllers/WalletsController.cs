using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.QuorumTransactionSigner.Client;
using Lykke.Service.QuorumTransactionSigner.Client.Models.Requests;
using Lykke.Service.QuorumTransactionSigner.Client.Models.Responses;
using Lykke.Service.QuorumTransactionSigner.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.QuorumTransactionSigner.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    public class WalletsController : ControllerBase, IQuorumTransactionSignerWalletsApi
    {
        private readonly IWalletService _walletService;

        
        public WalletsController(
            IWalletService walletService)
        {
            _walletService = walletService;
        }

        /// <summary>
        ///    Create new wallet.
        /// </summary>
        /// <returns>
        ///    Address of a wallet.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateWalletResponse), (int) HttpStatusCode.OK)]
        public async Task<CreateWalletResponse> CreateWalletAsync()
        {
            var response = new CreateWalletResponse
            {
                Address = await _walletService.CreateWalletAsync()
            };

            return response;
        }

        /// <summary>
        ///    Sign transaction with a specified address' private key.
        /// </summary>
        /// <returns>
        ///    Signed transaction in a hex format.
        /// </returns>
        [HttpPost("{address}/sign")]
        [ProducesResponseType(typeof(SignTransactionResponse), (int) HttpStatusCode.OK)]
        public async Task<SignTransactionResponse> SignTransactionAsync(
            [FromRoute] string address,
            [FromBody] SignTransactionRequest request)
        {
            address = address.ToLowerInvariant();

            var response = new SignTransactionResponse();

            if (await _walletService.WalletExistsAsync(address))
            {
                var rawTxHash = Convert.FromBase64String(request.RawTxHash);
                var (v, r, s) = await _walletService.SignTransactionAsync(address, rawTxHash);

                response.R = Convert.ToBase64String(r);
                response.S = Convert.ToBase64String(s);
                response.V = Convert.ToBase64String(v);
            }
            else
            {
                response.Error = SignTransactionError.WalletNotFound;
            }

            return response;
        }

        /// <summary>Sign transaction with a specified address' private key.</summary>
        /// <param name="address">Address of a signer (20 bytes) in a hex format (40 hexadecimal symbols) prefixed with 0x.</param>
        /// <param name="hashes">Request body, that contains raw transaction hash in the base64 encoding.</param>
        /// <returns>Sign transaction response, which contains signed transactions in a hex format.</returns>
        [HttpPost("{address}/batchsign")]
        [ProducesResponseType(typeof(SignTransactionsBatchResponse), (int)HttpStatusCode.OK)]
        public async Task<SignTransactionsBatchResponse> SignTransactionsBatchAsync([FromRoute] string address, [FromBody] List<string> hashes)
        {
            address = address.ToLowerInvariant();

            var response = new SignTransactionsBatchResponse();

            if (await _walletService.WalletExistsAsync(address))
            {
                var rawTxHashes = hashes.ToDictionary(i => i, Convert.FromBase64String);
                var signedDataDict = await _walletService.SignTransactionsAsync(address, rawTxHashes);

                response.HashToSignedTxDict = signedDataDict.ToDictionary(
                    p => p.Key,
                    p => new SignedTransactionData
                    {
                        R = Convert.ToBase64String(p.Value.R),
                        S = Convert.ToBase64String(p.Value.S),
                        V = Convert.ToBase64String(p.Value.V),
                    });
            }
            else
            {
                response.Error = SignTransactionError.WalletNotFound;
            }

            return response;
        }
    }
}
