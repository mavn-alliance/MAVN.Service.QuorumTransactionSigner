using JetBrains.Annotations;
using Lykke.HttpClientGenerator;

namespace MAVN.Service.QuorumTransactionSigner.Client
{
    /// <summary>
    ///    QuorumTransactionSigner API aggregating interface.
    /// </summary>
    [PublicAPI]
    public class QuorumTransactionSignerClient : IQuorumTransactionSignerClient
    {
        /// <summary>
        ///    Client constructor.
        /// </summary>
        public QuorumTransactionSignerClient(
            IHttpClientGenerator httpClientGenerator)
        {
            WalletsApi = httpClientGenerator.Generate<IQuorumTransactionSignerWalletsApi>();
        }

        /// <summary>
        ///    Wallets API interface.
        /// </summary>
        public IQuorumTransactionSignerWalletsApi WalletsApi { get; }
    }
}
