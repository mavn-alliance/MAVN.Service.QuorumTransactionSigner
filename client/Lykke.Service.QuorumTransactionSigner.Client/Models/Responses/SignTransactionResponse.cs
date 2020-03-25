using JetBrains.Annotations;

namespace Lykke.Service.QuorumTransactionSigner.Client.Models.Responses
{
    /// <summary>
    ///    A sign transaction response.
    /// </summary>
    [PublicAPI]
    public class SignTransactionResponse
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
        
        /// <summary>
        ///    Sign error.
        /// </summary>
        public SignTransactionError Error { get; set; }
    }
}
