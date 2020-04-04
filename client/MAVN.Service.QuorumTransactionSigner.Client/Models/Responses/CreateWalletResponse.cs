using JetBrains.Annotations;

namespace MAVN.Service.QuorumTransactionSigner.Client.Models.Responses
{
    /// <summary>
    ///    A create wallet response.
    /// </summary>
    [PublicAPI]
    public class CreateWalletResponse
    {
        /// <summary>
        ///    Address of the newly generated wallet.
        /// </summary>
        public string Address { get; set; }
    }
}
