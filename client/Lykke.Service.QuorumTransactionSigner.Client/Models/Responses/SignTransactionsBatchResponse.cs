using System.Collections.Generic;
using JetBrains.Annotations;

namespace Lykke.Service.QuorumTransactionSigner.Client.Models.Responses
{
    /// <summary>
    ///    Signed transactions batch response.
    /// </summary>
    [PublicAPI]
    public class SignTransactionsBatchResponse
    {
        /// <summary>Sign error.</summary>
        public SignTransactionError Error { get; set; }

        /// <summary>Hash to signed transaction data dictionary.</summary>
        public Dictionary<string, SignedTransactionData> HashToSignedTxDict { get; set; }
    }

    /// <summary>
    /// Signed transaction data.
    /// </summary>
    [PublicAPI]
    public class SignedTransactionData
    {
        /// <summary>
        ///    R parameter of the signature.
        /// </summary>
        public string R { get; set; }

        /// <summary>
        ///    S parameter of the signature.
        /// </summary>
        public string S { get; set; }

        /// <summary>
        ///    V parameter of the signature.
        /// </summary>
        public string V { get; set; }
    }
}
