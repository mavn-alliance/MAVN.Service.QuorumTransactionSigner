using JetBrains.Annotations;

namespace MAVN.Service.QuorumTransactionSigner.Client.Models.Responses
{
    /// <summary>
    ///    Sign transaction error enum
    /// </summary>
    [PublicAPI]
    public enum SignTransactionError
    {
        /// <summary>
        ///    Transaction has been signed without errors.
        /// </summary>
        None,
        
        /// <summary>
        ///    Wallet, that should sign a transaction, has not been found.
        /// </summary>
        WalletNotFound
    }
}
