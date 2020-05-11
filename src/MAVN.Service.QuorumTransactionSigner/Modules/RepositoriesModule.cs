using Autofac;
using JetBrains.Annotations;
using MAVN.Common.MsSql;
using MAVN.Service.QuorumTransactionSigner.Domain.Repositories;
using MAVN.Service.QuorumTransactionSigner.MsSqlRepositories;
using MAVN.Service.QuorumTransactionSigner.MsSqlRepositories.Contexts;
using MAVN.Service.QuorumTransactionSigner.Settings;
using Lykke.SettingsReader;

namespace MAVN.Service.QuorumTransactionSigner.Modules
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
