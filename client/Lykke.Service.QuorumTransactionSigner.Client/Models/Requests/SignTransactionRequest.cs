using JetBrains.Annotations;

namespace Lykke.Service.QuorumTransactionSigner.Client.Models.Requests
{
    /// <summary>
    ///    A sign transaction request.
    /// </summary>
    [PublicAPI]
    public class SignTransactionRequest
    {
        /// <summary>
        ///    Raw (not including signature) hash of a transaction.
        /// </summary>
        public string RawTxHash { get; set; }
    }
}
