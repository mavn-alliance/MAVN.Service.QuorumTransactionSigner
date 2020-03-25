using JetBrains.Annotations;

namespace Lykke.Service.QuorumTransactionSigner.Settings
{
    [UsedImplicitly]
    public class QuorumTransactionSignerSettings
    {
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public DbSettings Db { get; set; }
        
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public KeyVaultSettings KeyVault { get; set; }
    }
}
