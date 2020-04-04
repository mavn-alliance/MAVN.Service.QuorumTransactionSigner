using Autofac;
using JetBrains.Annotations;
using Lykke.Common.Log;
using MAVN.Service.QuorumTransactionSigner.Domain.Repositories;
using MAVN.Service.QuorumTransactionSigner.Domain.Services;
using MAVN.Service.QuorumTransactionSigner.DomainServices;
using MAVN.Service.QuorumTransactionSigner.Settings;
using Lykke.SettingsReader;
using Microsoft.Azure.KeyVault;

namespace MAVN.Service.QuorumTransactionSigner.Modules
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
