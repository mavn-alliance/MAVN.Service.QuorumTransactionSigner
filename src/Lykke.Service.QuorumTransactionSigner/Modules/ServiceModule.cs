using Autofac;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.QuorumTransactionSigner.Domain.Repositories;
using Lykke.Service.QuorumTransactionSigner.Domain.Services;
using Lykke.Service.QuorumTransactionSigner.DomainServices;
using Lykke.Service.QuorumTransactionSigner.Settings;
using Lykke.SettingsReader;
using Microsoft.Azure.KeyVault;

namespace Lykke.Service.QuorumTransactionSigner.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(
            IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(
            ContainerBuilder builder)
        {
            var keyVaultSettings = _appSettings.CurrentValue.QuorumTransactionSignerService.KeyVault;

            builder
                .Register(ctx => new WalletService
                (
                    ctx.Resolve<IKeyVaultClient>(),
                    ctx.Resolve<ILogFactory>(),
                    keyVaultSettings.BaseUrl,
                    ctx.Resolve<IWalletRepository>(),
                    keyVaultSettings.BatchSize
                ))
                .As<IWalletService>()
                .SingleInstance();
        }
    }
}
