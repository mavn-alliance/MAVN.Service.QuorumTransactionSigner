using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.QuorumTransactionSigner.Domain.Repositories;
using Lykke.Service.QuorumTransactionSigner.Domain.Services;
using Lykke.Service.QuorumTransactionSigner.DomainServices.Extensions;
using Microsoft.Azure.KeyVault;
using MoreLinq;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Signer.Crypto;

namespace Lykke.Service.QuorumTransactionSigner.DomainServices
{
    public class WalletService : IWalletService
    {
        private const int DefaultBatchSize = 50;

        private readonly IKeyVaultClient _keyVaultClient;
        private readonly ILog _log;
        private readonly string _vaultBaseUrl;
        private readonly IWalletRepository _walletRepository;
        private readonly int _batchSize;

        public WalletService(
            IKeyVaultClient keyVaultClient,
            ILogFactory logFactory,
            string vaultBaseUrl,
            IWalletRepository walletRepository,
            int? batchSize = null)
        {
            _keyVaultClient = keyVaultClient;
            _log = logFactory.CreateLog(this);
            _vaultBaseUrl = vaultBaseUrl;
            _walletRepository = walletRepository;
            _batchSize = batchSize ?? DefaultBatchSize;
        }

        public async Task<string> CreateWalletAsync()
        {
            using (_log.BeginScope($"{nameof(CreateWalletAsync)}-{Guid.NewGuid()}"))
            {
                try
                {
                    var (address, publicKey) = await GenerateAndImportPrivateKeyAsync();

                    try
                    {
                        await _walletRepository.SaveWalletAsync(address, publicKey);

                        #region Logging

                        _log.Info
                        (
                            $"Wallet [{address}] has been generated.",
                            new {address}
                        );

                        #endregion

                        return address;
                    }
                    catch (Exception)
                    {
                        await TryCleanupKeyAsync(address);

                        throw;
                    }
                }
                catch (Exception e)
                {
                    #region Logging
                    
                    _log.Error(e, "Wallet generation failed.");
                    
                    #endregion

                    throw;
                }
            }
        }

        public async Task<(byte[] V, byte[] R, byte[] S)> SignTransactionAsync(string address, byte[] rawTxHash)
        {
            var keyIdentifier = new KeyIdentifier
            (
                vaultBaseUrl: _vaultBaseUrl,
                name: address
            );
            var publicKey = await _walletRepository.GetPublicKeyAsync(address);

            var signedData = await SignTransactionAsync(
                keyIdentifier,
                null,
                rawTxHash,
                publicKey);

            return (signedData.Item2, signedData.Item3, signedData.Item4);
        }

        public async Task<Dictionary<string, (byte[] V, byte[] R, byte[] S)>> SignTransactionsAsync(
            string address, Dictionary<string, byte[]> rawTxHashes)
        {
            var result = new Dictionary<string, (byte[] V, byte[] R, byte[] S)>();
            var keyIdentifier = new KeyIdentifier
            (
                vaultBaseUrl: _vaultBaseUrl,
                name: address
            );
            var publicKey = await _walletRepository.GetPublicKeyAsync(address);
            var batches = rawTxHashes.Batch(_batchSize);

            foreach (var batch in batches)
            {
                var tasks = new List<Task<(string, byte[], byte[], byte[])>>();
                tasks.AddRange(
                    batch.Select(b => SignTransactionAsync(
                        keyIdentifier,
                        b.Key,
                        b.Value,
                        publicKey)));
                await Task.WhenAll(tasks);

                foreach (var task in tasks)
                {
                    var signedData = task.Result;
                    result.Add(signedData.Item1, (signedData.Item2, signedData.Item3, signedData.Item4));
                }
            }

            return result;
        }

        public Task<bool> WalletExistsAsync(string address)
        {
            return _walletRepository.WalletExistsAsync(address);
        }

        private async Task<(string, byte[], byte[], byte[])> SignTransactionAsync(
            KeyIdentifier keyIdentifier,
            string txHash,
            byte[] rawTxHash,
            byte[] publicKey)
        {
            using (_log.BeginScope($"{nameof(SignTransactionAsync)}-{Guid.NewGuid()}"))
            {
                try
                {
                    var rs = await _keyVaultClient.SignAsync
                    (
                        keyIdentifier: keyIdentifier.Identifier,
                        algorithm: "ECDSA256",
                        digest: rawTxHash
                    );

                    var ecdsaSignature = ECDSASignatureFactory.FromComponents(rs.Result).MakeCanonical();
                    var recId = CalculateRecId(ecdsaSignature, rawTxHash, publicKey);

                    var r = ecdsaSignature.R.ToByteArray();
                    var s = ecdsaSignature.S.ToByteArray();
                    var v = new[] { (byte)(recId + 27) };

                    #region Logging

                    _log.Info
                    (
                        "Transaction has been signed.",
                        new { address = keyIdentifier.Name, rawTxHash = rawTxHash.ToHex(true) }
                    );

                    #endregion

                    return (txHash, v, r, s);
                }
                catch (Exception e)
                {
                    #region Logging

                    _log.Error
                    (
                        e,
                        "Transaction signing failed.",
                        new { address = keyIdentifier.Name, rawTxHash = rawTxHash.ToHex(true) }
                    );

                    #endregion

                    throw;
                }
            }
        }

        private static int CalculateRecId(
            ECDSASignature signature,
            byte[] hash,
            byte[] publicKey)
        {
            var num = -1;

            for (var recId = 0; recId < 4; ++recId)
            {
                var ecKey = ECKey.RecoverFromSignature(recId, signature, hash, false);
                var pubKey = ecKey?.GetPubKey(false);
                
                if (pubKey != null && pubKey.SequenceEqual(publicKey))
                {
                    num = recId;
                    break;
                }
            }

            if (num == -1)
                throw new Exception("Could not construct a recoverable key. This should never happen.");

            return num;
        }

        private async Task<(string, byte[])> GenerateAndImportPrivateKeyAsync()
        {
            var ecKey = EthECKey.GenerateKey();

            var address = ecKey.GetPublicAddress().ToLowerInvariant();

            #region Logging

            _log.Debug
            (
                $"A private key for {address} has been generated.",
                new {address}
            );

            #endregion

            await _keyVaultClient.ImportKeyAsync
            (
                vaultBaseUrl: _vaultBaseUrl,
                keyName: address,
                key: ecKey.ToJsonWebKey()
            );

            #region Logging

            _log.Debug
            (
                $"A private key for {address} has been imported to the key vault.",
                new {address}
            );

            #endregion

            return (address, ecKey.GetPubKey());
        }

        private async Task TryCleanupKeyAsync(
            string address)
        {
            try
            {
                await _keyVaultClient.DeleteKeyAsync(_vaultBaseUrl, address);
            }
            catch (Exception e)
            {
                #region Logging

                _log.Warning
                (
                    $"Failed to remove private key for {address} from key vault after wallet generation error.",
                    e,
                    new {address}
                );

                #endregion
            }
        }
    }
}
