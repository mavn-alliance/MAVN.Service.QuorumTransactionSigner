using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.QuorumTransactionSigner.Client 
{
    /// <summary>
    /// QuorumTransactionSigner client settings.
    /// </summary>
    [PublicAPI]
    public class QuorumTransactionSignerServiceClientSettings 
    {
        /// <summary>
        ///    Service url.
        /// </summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl { get; set;}
    }
}
