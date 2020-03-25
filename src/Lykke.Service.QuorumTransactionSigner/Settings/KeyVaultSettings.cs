using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.QuorumTransactionSigner.Settings
{
    [UsedImplicitly]
    public class KeyVaultSettings
    {
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public string AuthConnString { get; set; }

        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public string BaseUrl { get; set; }

        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public string PrivateKeyBackupEncryptionKeyName { get; set; }

        [Optional]
        public int? BatchSize { get; set; }
    }
}
