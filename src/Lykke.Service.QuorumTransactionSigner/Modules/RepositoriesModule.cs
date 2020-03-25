using Autofac;
using JetBrains.Annotations;
using Lykke.Common.MsSql;
using Lykke.Service.QuorumTransactionSigner.Domain.Repositories;
using Lykke.Service.QuorumTransactionSigner.MsSqlRepositories;
using Lykke.Service.QuorumTransactionSigner.MsSqlRepositories.Contexts;
using Lykke.Service.QuorumTransactionSigner.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.QuorumTransactionSigner.Modules
{
    [UsedImplicitly]
    public class RepositoriesModule : Module
    {
        private readonly DbSettings _dbSettings;

        public RepositoriesModule(
            IReloadingManager<AppSettings> appSettings)
        {
            _dbSettings = appSettings.CurrentValue.QuorumTransactionSignerService.Db;
        }

        protected override void Load(
            ContainerBuilder builder)
        {
            builder
                .RegisterMsSql(() => new QtsContext(_dbSettings.DataConnString, false));

            builder
                .Register(ctx => new WalletRepository
                (
                    ctx.Resolve<MsSqlContextFactory<QtsContext>>()
                ))
                .As<IWalletRepository>()
                .SingleInstance();
        }
    }
}
