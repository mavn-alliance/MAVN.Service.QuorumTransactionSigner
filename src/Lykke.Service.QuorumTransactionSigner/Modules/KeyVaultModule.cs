using Autofac;
using JetBrains.Annotations;
using Lykke.Service.QuorumTransactionSigner.Settings;
using Lykke.SettingsReader;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace Lykke.Service.QuorumTransactionSigner.Modules
{
    [UsedImplicitly]
    public class KeyVaultModule : Module
    {
        private readonly KeyVaultSettings _keyVaultSettings;
        
        public KeyVaultModule(
            IReloadingManager<AppSettings> appSettings)
        {
            _keyVaultSettings = appSettings.CurrentValue.QuorumTransactionSignerService.KeyVault;
        }

        protected override void Load(
            ContainerBuilder builder)
        {
            builder
                .Register(ctx =>
                {
                    var authTokenProvider = new AzureServiceTokenProvider(_keyVaultSettings.AuthConnString);
                    var authCallback = new KeyVaultClient.AuthenticationCallback(authTokenProvider.KeyVaultTokenCallback);

                    return new KeyVaultClient(authCallback);
                })
                .As<IKeyVaultClient>()
                .SingleInstance();
        }
    }
}
