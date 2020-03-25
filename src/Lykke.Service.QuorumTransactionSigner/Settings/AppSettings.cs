using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Service.QuorumTransactionSigner.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public QuorumTransactionSignerSettings QuorumTransactionSignerService { get; set; }
    }
}
