using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.QuorumTransactionSigner.Settings
{
    [UsedImplicitly]
    public class DbSettings
    {
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public string DataConnString { get; set; }
        
        [AzureTableCheck]
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public string LogsConnString { get; set; }
    }
}
