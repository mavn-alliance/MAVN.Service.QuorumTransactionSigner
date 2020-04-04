using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace MAVN.Service.QuorumTransactionSigner.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public QuorumTransactionSignerSettings QuorumTransactionSignerService { get; set; }
    }
}
