using JetBrains.Annotations;

namespace MAVN.Service.QuorumTransactionSigner.Client
{
    /// <summary>
    ///    QuorumTransactionSigner client interface.
    /// </summary>
    [PublicAPI]
    public interface IQuorumTransactionSignerClient
    {
        /// <summary>
        ///    Wallets API interface.
        /// </summary>
        IQuorumTransactionSignerWalletsApi WalletsApi { get; }
    }
}
