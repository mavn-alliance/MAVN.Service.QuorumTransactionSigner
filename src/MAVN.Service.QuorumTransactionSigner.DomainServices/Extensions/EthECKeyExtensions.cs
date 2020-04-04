using Microsoft.Azure.KeyVault.WebKey;
using Nethereum.Signer;
using Nethereum.Util;

namespace MAVN.Service.QuorumTransactionSigner.DomainServices.Extensions
{
    internal static class EthECKeyExtensions
    {
        public static JsonWebKey ToJsonWebKey(
            this EthECKey ecKey)
        {
            var privateKey = ecKey.GetPrivateKeyAsBytes();
            var publicKey = ecKey.GetPubKey();
            
            var ecParameters = new ECParameters
            {
                Curve = "SECP256K1",
                D = privateKey,
                X = publicKey.Slice(1, 33),
                Y = publicKey.Slice(33, 65)
            };

            return new JsonWebKey(ecParameters);
        }
    }
}
